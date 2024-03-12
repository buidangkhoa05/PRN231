using BusinessObject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ApiHelpers
{
	public interface IApiHelper : IDisposable
	{
		Task<ApiResponse<T>> GetAsync<T>(string url, object parameters = null);
		Task<ApiResponse<T>> GetAsync<T>(string url, Dictionary<string, string> parameters = null);
		Task<ApiResponse<T>> PostAsync<T>(string url, object dataToPost = null);
		Task<ApiResponse<T>> PostFileAsync<T>(string url, MultipartFormDataContent formDataContent);
		Task<ApiResponse<T>> DeleteAsync<T>(string url);
		Task<ApiResponse<T>> PutAsync<T>(string url, object dataToPut = null);
	}
}
