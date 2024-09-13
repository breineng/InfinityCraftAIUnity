using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LumaGenie.Data.Convert;
using LumaGenie.Data.Creation;
using LumaGenie.Data.Status;
using Newtonsoft.Json;
using Random = System.Random;

namespace LumaGenie
{
    public class GenieClient
    {
        private const string API_URL = "https://webapp.engineeringlumalabs.com/api/v3/";
        private const string CREATIONS_URL = API_URL + "creations";
        private const string CONVERT_URL = CREATIONS_URL + "/convert";
        private const string STATUS_URL = CREATIONS_URL + "/uuid/{0}";
        
        private readonly HttpClient _httpClient;
        private string _token;

        /// <summary>
        /// Initializes a new instance of the GenieClient class with the specified token.
        /// </summary>
        /// <param name="token">The authorization token to be used for API requests.</param>
        public GenieClient(string token)
        {
            _httpClient = new HttpClient();
            _token = token;
        }

        /// <summary>
        /// Updates the authorization token.
        /// </summary>
        /// <param name="token">The new authorization token.</param>
        public void UpdateToken(string token)
        {
            _token = token;
        }
        
        /// <summary>
        /// Sends a creation request asynchronously.
        /// </summary>
        /// <param name="creationRequest">The creation request object.</param>
        /// <returns>A task representing the asynchronous operation, containing the creation response.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the token is not set.</exception>
        public async Task<CreationResponse> CreationRequestAsync(CreationRequest creationRequest)
        {
            if (string.IsNullOrEmpty(_token))
                throw new InvalidOperationException("Authorize before creation request");
            
            creationRequest.Input.JobParams.Seed ??= GenerateRandomSeed();
            string jsonBody = JsonConvert.SerializeObject(creationRequest);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, CREATIONS_URL);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
            request.Content = content;
            HttpResponseMessage response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CreationResponse>(responseBody);
        }

        /// <summary>
        /// Sends a conversion request asynchronously.
        /// </summary>
        /// <param name="convertRequest">The convert request object.</param>
        /// <returns>A task representing the asynchronous operation, containing the convert response.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the token is not set.</exception>
        public async Task<ConvertResponse?> ConvertRequestAsync(ConvertRequest convertRequest)
        {
            string jsonBody = JsonConvert.SerializeObject(convertRequest);
            HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(CONVERT_URL, content);

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ConvertResponse>(responseBody);
        }
        
        /// <summary>
        /// Gets the status of a creation by UUID asynchronously.
        /// </summary>
        /// <param name="uuid">The UUID of the creation.</param>
        /// <returns>A task representing the asynchronous operation, containing the status response.</returns>
        public async Task<StatusResponse?> GetStatusRequestAsync(string uuid)
        {
            string url = string.Format(STATUS_URL, uuid);
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StatusResponse>(responseBody);
        }

        /// <summary>
        /// Downloads a ZIP file from the specified URL and extracts its contents asynchronously.
        /// </summary>
        /// <param name="fileUrl">The URL of the ZIP file to download.</param>
        /// <param name="onFileExtracted">The action to perform on each extracted file.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<List<(string Name, Stream Content)>> DownloadAndExtractFileAsync(string fileUrl)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(fileUrl);
            response.EnsureSuccessStatusCode();

            byte[] zipData = await response.Content.ReadAsByteArrayAsync();
            var streams = new List<(string Name, Stream Content)>();
            using MemoryStream zipStream = new MemoryStream(zipData);
            using ZipArchive archive = new ZipArchive(zipStream);
            
            foreach (var entry in archive.Entries)
            {
                MemoryStream entryStream = new MemoryStream();
                using Stream stream = entry.Open();
                await stream.CopyToAsync(entryStream);
                entryStream.Seek(0, SeekOrigin.Begin);
                streams.Add((entry.Name, entryStream));
            }

            return streams;
        }
        
        /// <summary>
        /// Generates a random seed.
        /// </summary>
        /// <returns>A random seed.</returns>
        public static string GenerateRandomSeed()
        {
            var random = new Random();
            return random.Next(000000000, 999999999).ToString();
        }
    }
}
