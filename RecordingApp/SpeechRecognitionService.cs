using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RecordingApp.Services
{
    public class SpeechRecognitionService
    {
        private readonly string _apiKey;
        private readonly string _apiUrl;

        public SpeechRecognitionService(string apiKey, string apiUrl)
        {
            _apiKey = apiKey;
            _apiUrl = apiUrl;
        }

        public async Task<string> RecognizeSpeechFromFileAsync(string filePath)
        {
            try
            {
                using var httpClient = new HttpClient
                {
                    Timeout = TimeSpan.FromMinutes(5)
                };

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

                using var content = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");

                content.Add(fileContent, "file", Path.GetFileName(filePath));
                content.Add(new StringContent("whisper-1"), "model");
                content.Add(new StringContent("text"), "response_format");

                var response = await httpClient.PostAsync(_apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Exception: {ex.Message}");
            }
        }
    }
}
