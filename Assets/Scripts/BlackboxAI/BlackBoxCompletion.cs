using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace InfinityCraft.BlackboxAI
{

	public class BlackBoxCompletion
	{
		private const string Url = "https://www.blackbox.ai/api/chat";
		private readonly HttpClient _httpClient;

		public BlackBoxCompletion()
		{
			_httpClient = new HttpClient();
		}

		public async Task<string> Create(string message)
		{
			var headers = new[]
			{
			new KeyValuePair<string, string>("accept", "*/*"),
			new KeyValuePair<string, string>("accept-language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7"),
			new KeyValuePair<string, string>("content-type", "application/json"),
			new KeyValuePair<string, string>("origin", "https://www.blackbox.ai"),
			new KeyValuePair<string, string>("priority", "u=1, i"),
			new KeyValuePair<string, string>("referer", "https://www.blackbox.ai/"),
			new KeyValuePair<string, string>("sec-ch-ua", "\"Chromium\";v=\"128\", \"Not;A=Brand\";v=\"24\", \"Google Chrome\";v=\"128\""),
			new KeyValuePair<string, string>("sec-ch-ua-mobile", "?0"),
			new KeyValuePair<string, string>("platform", "Windows"),
			new KeyValuePair<string, string>("sec-fetch-dest", "empty"),
			new KeyValuePair<string, string>("sec-fetch-mode", "cors"),
			new KeyValuePair<string, string>("sec-fetch-site", "same-origin"),
			new KeyValuePair<string, string>("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/128.0.0.0 Safari/537.36")
		};

			var data = new
			{
				messages = new[]
				{
				new
				{
					id = Guid.NewGuid().ToString(),
					content = message,
					role = "user"
				}
			},
				id = Guid.NewGuid().ToString(),
				codeModelMode = true,
				agentMode = new { },
				trendingAgentMode = new { },
				isMicMode = false,
				maxTokens = 1024,
				isChromeExt = false,
				clickedAnswer2 = false,
				clickedAnswer3 = false,
				clickedForceWebSearch = false,
				visitFromDelta = false,
				mobileClient = false
			};

			var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(Url, content);
			if (response.IsSuccessStatusCode)
			{
				var responseBody = await response.Content.ReadAsStringAsync();
                return responseBody.Substring(responseBody.LastIndexOf("@$") + 2);
            }
			else
			{
				string responseBody = await response.Content.ReadAsStringAsync();
				Debug.LogError(responseBody);
				return null;
			}
		}
	}
}