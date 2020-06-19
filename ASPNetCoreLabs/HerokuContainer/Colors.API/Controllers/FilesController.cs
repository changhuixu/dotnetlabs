using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Colors.API.Controllers
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Download a file. This demo will generate a txt file.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Download a File by FileID")]
        public IActionResult Download(int id)
        {
            return File(Encoding.ASCII.GetBytes("hello world"), "text/plain", $"test-{id}.txt");
        }

        /// <summary>
        /// Upload a file. This demo is dummy and only waits 2 seconds.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("single-file")]
        public async Task Upload(IFormFile file)
        {
            _logger.LogInformation($"validating the file {file.FileName}");
            _logger.LogInformation("saving file");
            await Task.Delay(2000); // validate file type/format/size, scan virus, save it to a storage
            _logger.LogInformation("file saved.");
        }

        /// <summary>
        /// Upload two files. This demo is dummy and only waits 2 seconds.
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        [HttpPost("two-files")]
        public async Task Upload(IFormFile file1, IFormFile file2)
        {
            _logger.LogInformation($"validating the file {file1.FileName}");
            _logger.LogInformation($"validating the file {file2.FileName}");
            _logger.LogInformation("saving files");
            await Task.Delay(2000);
            _logger.LogInformation("files saved.");
        }

        /// <summary>
        /// Upload multiple files. This demo is dummy and only waits 2 seconds.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost("multiple-files")]
        public async Task Upload(List<IFormFile> files)
        {
            _logger.LogInformation($"validating {files.Count} files");
            foreach (var file in files)
            {
                _logger.LogInformation($"saving file {file.FileName}");
                await Task.Delay(1000);
            }
            _logger.LogInformation("All files saved.");
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        public async Task<bool> Delete(int id)
        {
            _logger.LogInformation($"deleting file ID=[{id}]");
            await Task.Delay(1500);
            return true;
        }
    }
}
