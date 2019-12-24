using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ServerSideApp.Filters;
using ServerSideApp.Utilities;

namespace ServerSideApp.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;
        private readonly long _fileSizeLimit;
        private readonly string[] _permittedExtensions = { ".txt" };
        private readonly string _targetFolderPath;
        private static readonly List<MyFormModel> MyFiles = new List<MyFormModel>();

        public FilesController(ILogger<FilesController> logger, IConfiguration config)
        {
            _logger = logger;
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");

            _targetFolderPath = config.GetValue<string>("StoredFilesPath");   // To save physical files to a path provided by configuration:
            //_targetFolderPath = Path.GetTempPath(); // To save physical files to the temporary files folder
            Directory.CreateDirectory(_targetFolderPath);
        }

        [HttpPost("")]
        [DisableFormValueModelBinding]
        public async Task<IActionResult> Upload()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File", "The request couldn't be processed (Error 1).");
                _logger.LogWarning($"The request content type [{Request.ContentType}] is invalid.");
                return BadRequest(ModelState);
            }

            var formModel = new MyFormModel();

            var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), new FormOptions().MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (contentDisposition.IsFileDisposition())
                    {
                        // Don't trust the file name sent by the client. To display the file name, HTML-encode the value.
                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(contentDisposition.FileName.Value);
                        var trustedFileNameForFileStorage = Path.GetRandomFileName();

                        // todo: scan the file's contents using an anti-virus/anti-malware scanner API

                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(section, contentDisposition, ModelState, _permittedExtensions, _fileSizeLimit);

                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }

                        var trustedFilePath = Path.Combine(_targetFolderPath, trustedFileNameForFileStorage);
                        using (var targetStream = System.IO.File.Create(trustedFilePath))
                        {
                            await targetStream.WriteAsync(streamedFileContent);
                            formModel.TrustedFilePath = trustedFilePath;
                            formModel.TrustedFileName = trustedFileNameForDisplay;
                            _logger.LogInformation($"Uploaded file '{trustedFileNameForDisplay}' saved to '{_targetFolderPath}' as {trustedFileNameForFileStorage}");
                        }
                    }
                    else if (contentDisposition.IsFormDisposition())
                    {
                        var content = new StreamReader(section.Body).ReadToEnd();
                        if (contentDisposition.Name == "userId" && int.TryParse(content, out var useId))
                        {
                            formModel.UserId = useId;
                        }

                        if (contentDisposition.Name == "comment")
                        {
                            formModel.Comment = content;
                        }

                        if (contentDisposition.Name == "isPrimary" && bool.TryParse(content, out var isPrimary))
                        {
                            formModel.IsPrimary = isPrimary;
                        }
                    }
                }

                // Drain any remaining section body that hasn't been consumed and read the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            // todo: validate and persist formModel
            _logger.LogInformation(formModel.ToString());
            MyFiles.Add(formModel);

            return Created(nameof(FilesController), formModel);
        }

        [HttpGet("")]
        public async Task<IActionResult> Download([FromQuery]string guid)
        {
            var myFile = MyFiles.FirstOrDefault(x => x.Guid == guid);
            if (myFile == null)
            {
                _logger.LogInformation($"File with GUID={guid} Not Found.");
                return BadRequest($"File with GUID={guid} Not Found.");
            }

            var filePath = myFile.TrustedFilePath;
            _logger.LogInformation($"downloading file [{filePath}].");
            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, GetMimeTypes(Path.GetExtension(myFile.TrustedFileName)), myFile.TrustedFileName);
        }

        private static string GetMimeTypes(string ext)
        {
            switch (ext)
            {
                case ".txt": return "text/plain";
                case ".csv": return "text/csv";
                case ".pdf": return "application/pdf";
                case ".doc": return "application/vnd.ms-word";
                case ".xls": return "application/vnd.ms-excel";
                case ".ppt": return "application/vnd.ms-powerpoint";
                case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".png": return "image/png";
                case ".jpg": return "image/jpeg";
                case ".jpeg": return "image/jpeg";
                case ".gif": return "image/gif";
                default: return "application/octet-stream";
            }
        }
    }

    public class MyFormModel
    {
        public string TrustedFilePath { get; set; }
        public string TrustedFileName { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; }
        public string Guid { get; private set; } = System.Guid.NewGuid().ToString();
        public bool IsPrimary { get; set; }

        public override string ToString()
        {
            return $"{nameof(TrustedFilePath)}: [{TrustedFilePath}];" + Environment.NewLine +
                   $"{nameof(UserId)}: {UserId}; " + Environment.NewLine +
                   $"{nameof(Guid)}: {Guid}; " + Environment.NewLine +
                   $"{nameof(IsPrimary)}: {IsPrimary}; ";
        }
    }
}
