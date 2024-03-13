using BusinessObject.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Service.Common;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Service.ApiHelpers
{
	public class ApiHelper : IApiHelper
	{
		private readonly HttpClient _client;
		//private readonly ILogger _logger;

		//private static string _token { get; set; }

		private bool _disposedValue = false;

		public ApiHelper(HttpClient client)
		{
			_client = client;
			//_logger = logger;
		}

		//private void Init()
		//{
		//	_token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
		//}

		public async Task<ApiResponse<T>> GetAsync<T>(string url, object? parameters = null)
		{
			var query = parameters != null ? parameters.ToQueryString() : string.Empty;
			var requestMessage = await GetHttpRequestMessage(HttpMethod.Get, url + query);

			return await GetResources<T>(requestMessage, query);
		}

		public async Task<ApiResponse<T>> GetAsync<T>(string url, Dictionary<string, string>? parameters = null)
		{
			var query = parameters != null ? parameters.ToQueryString() : string.Empty;
			var requestMessage = await GetHttpRequestMessage(HttpMethod.Get, url + query);

			return await GetResources<T>(requestMessage, query);
		}

		public async Task<ApiResponse<T>> PostAsync<T>(string url, object? dataToPost = null)
		{
			var requestMessage = await GetHttpRequestMessage(HttpMethod.Post, url);

			var objRequest = JsonConvert.SerializeObject(dataToPost, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
			requestMessage.Content = new StringContent(objRequest, Encoding.UTF8, "application/json");

			return await GetResources<T>(requestMessage, objRequest);
		}

		public async Task<ApiResponse<T>> PostFileAsync<T>(string url, MultipartFormDataContent formDataContent)
		{
			var requestMessage = await GetHttpRequestMessage(HttpMethod.Post, url);
			requestMessage.Content = formDataContent;

			return await GetResources<T>(requestMessage);
		}

		public async Task<ApiResponse<T>> DeleteAsync<T>(string url)
		{
			var requestMessage = await GetHttpRequestMessage(HttpMethod.Delete, url);

			return await GetResources<T>(requestMessage, string.Empty);
		}

		public async Task<ApiResponse<T>> PutAsync<T>(string url, object? dataToPut = null)
		{
			var requestMessage = await GetHttpRequestMessage(HttpMethod.Put, url);

			var objRequest = JsonConvert.SerializeObject(dataToPut, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
			requestMessage.Content = new StringContent(objRequest, Encoding.UTF8, "application/json");

			return await GetResources<T>(requestMessage, objRequest);
		}

		private async Task<HttpRequestMessage> GetHttpRequestMessage(HttpMethod method, string url)
		{
			//Init();

			_client.DefaultRequestHeaders.Clear();
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Constant.token);
			_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			var request = new HttpRequestMessage(method, url);

			await Task.CompletedTask;

			return request;
		}

		private async Task<ApiResponse<T>> GetResources<T>(HttpRequestMessage requestMessage, string? jsonBody = null)
		{
			var result = new ApiResponse<T>();

			using (var response = await _client.SendAsync(requestMessage))
			{
				try
				{
					if (response.StatusCode == HttpStatusCode.OK)
					{
						var data = await GetResponse<T>(response);
						result.Data = data;
					}

					result.Message = response.ReasonPhrase;
					result.StatusCode = response.StatusCode;
				}
				catch (Exception ex)
				{
					return null;
				}
			}
			return result;
		}

		private async Task<T?> GetResponse<T>(HttpResponseMessage response)
		{
			using (var content = response.Content)
			{
				var responseData = await content.ReadAsStringAsync();

				return JsonConvert.DeserializeObject<T>(responseData);
			}
		}

		#region Destructor
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing)
				{
					_client.Dispose();
				}
				_disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ApiHelper()
		{
			Dispose(false);
		}
		#endregion
	}
}
