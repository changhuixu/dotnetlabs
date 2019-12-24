using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ClientSideApp
{
    public interface IDemoHttpClient
    {
        Task<string> UploadFile(string filePath);
        Task<string> DownloadFile(string guid);
    }

    public class DemoHttpClient : IDemoHttpClient
    {
        private readonly ILogger<DemoHttpClient> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _url;

        public DemoHttpClient(HttpClient httpClient, ILogger<DemoHttpClient> logger, DemoHttpClientSettings settings)
        {
            _httpClient = httpClient;
            _logger = logger;
            _url = settings.Url;
        }

        public async Task<string> UploadFile(string filePath)
        {
            _logger.LogInformation($"Uploading a text file [{filePath}].");
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File [{filePath}] not found.");
            }
            using var form = new MultipartFormDataContent();
            using var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            form.Add(fileContent, "file", Path.GetFileName(filePath));
            form.Add(new StringContent("789"), "userId");
            form.Add(new StringContent("some comments"), "comment");
            form.Add(new StringContent("true"), "isPrimary");

            var response = await _httpClient.PostAsync($"{_url}/api/files", form);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<FileUploadResult>(responseContent);
            _logger.LogInformation("Uploading is complete.");
            return result.Guid;
        }

        public async Task<string> DownloadFile(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                throw new ArgumentNullException(nameof(guid), "GUID is empty.");
            }
            _logger.LogInformation($"Downloading File with GUID=[{guid}].");
            var fileInfo = new FileInfo($"{guid}.txt");

            var response = await _httpClient.GetAsync($"{_url}/api/files?guid={guid}");
            response.EnsureSuccessStatusCode();
            await using var ms = await response.Content.ReadAsStreamAsync();
            await using var fs = File.Create(fileInfo.FullName);
            ms.Seek(0, SeekOrigin.Begin);
            ms.CopyTo(fs);

            _logger.LogInformation($"File saved as [{fileInfo.Name}].");
            return fileInfo.FullName;
        }
    }

    public class FileUploadResult
    {
        [JsonPropertyName("guid")]
        public string Guid { get; set; }
    }

    public class DemoHttpClientSettings
    {
        public string Url { get; set; }
    }
}
