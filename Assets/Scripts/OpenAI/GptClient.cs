using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace InfinityCraft.OpenAI
{
    public class GptClient
    {
        private const string API_URL = "https://api.openai.com/v1/chat/completions";
        private string _apiKey;

        public GptClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        public void UpdateKey(string key)
        {
            _apiKey = key;
        }

        public async Task<ApiResponse> Completions(string instructions, string prompt)
        {
            var requestData = new
            {
                model = "gpt-4o-mini",
                messages = new object[]
                {
                    new
                    {
                        role = "system",
                        content = new object[]
                        {
                            new
                            {
                                type = "text",
                                text = instructions
                            }
                        }
                    },
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new
                            {
                                type = "text",
                                text = prompt
                            }
                        }
                    }
                },
                temperature = 1,
                max_tokens = 256,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0,
                response_format = new
                {
                    type = "text"
                }
            };

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpClient client = new HttpClient();
            
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            HttpResponseMessage response = await client.PostAsync(API_URL, content);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(responseBody);
                return apiResponse;
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.LogError(responseBody);
                return null;
            }
        }
        
        public static async Task<bool> VerifyApiKey(string apiKey)
        {
            using HttpClient client = new HttpClient();
            
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(API_URL, content);
            return response.StatusCode == HttpStatusCode.BadRequest;
        }
    }
}