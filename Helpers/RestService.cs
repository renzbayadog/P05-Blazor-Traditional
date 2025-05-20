using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace codegen.Helpers
{
    public class RestService<TReturn>
    {
        public RestService()
        {
        }


        /// <summary>
        /// Get Request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ServiceModel<TReturn>> Get(string url)
        {
            var result = new ServiceModel<TReturn>();
            var uri = new Uri(string.Format(url));

            result = await GetAPI(uri);

            return result;
        }

        /// <summary>
        /// Post Request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ServiceModel<TReturn>> Post(string url, string param)
        {
            var result = new ServiceModel<TReturn>();
            var uri = new Uri(string.Format(url));

            result = await PostAPI(uri, param);
            return result;
        }

        /// <summary>
        /// Put Request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ServiceModel<TReturn>> Put(string url, string param = null)
        {
            var result = new ServiceModel<TReturn>();
            var uri = new Uri(string.Format(url));

            var paramcontent = new StringContent(param, Encoding.UTF8, "application/json");

            result = await PutAPI(uri, param);
            return result;
        }

        /// <summary>
        /// Delete Request
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ServiceModel<TReturn>> Delete(string url)
        {
            var result = new ServiceModel<TReturn>();
            var uri = new Uri(string.Format(url));

            result = await DeleteAPI(uri);

            return result;
        }


        #region Private Methods
        /// <summary>
        /// GET API call
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<ServiceModel<TReturn>> GetAPI(Uri uri, string refreshToken = null)
        {
            var result = new ServiceModel<TReturn>();

            try
            {
                using (var _client = new HttpClient())
                {
                    var token = SetToken(refreshToken);

                    if (token != null)
                    {
                        _client.DefaultRequestHeaders.Authorization = token;
                    }

                    var response = await _client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        result.Data = JsonConvert.DeserializeObject<TReturn>(content);
                        result.IsSuccess = true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        result.IsSuccess = false;
                    }
                    else
                    {
                        var ex = APIExceptionHelper.CreateExceptionFromResponseErrors(response);
                        result.Error = ex.Message;
                        result.IsSuccess = false;
                    }

                    result.StatusCode = response.StatusCode;
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Error = "An error has occured please contact the administrator.";
            }

            return result;
        }

        /// <summary>
        /// POST API call
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="param"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<ServiceModel<TReturn>> PostAPI(Uri uri, string param, string refreshToken = null)
        {
            var result = new ServiceModel<TReturn>();
            var paramcontent = new StringContent(param, Encoding.UTF8, "application/json");

            try
            {
                using (var _client = new HttpClient())
                {
                    var token = SetToken(refreshToken);

                    if (token != null)
                    {
                        _client.DefaultRequestHeaders.Authorization = token;
                    }

                    var response = await _client.PostAsync(uri, paramcontent);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        result.Data = JsonConvert.DeserializeObject<TReturn>(content);
                        result.IsSuccess = true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        result.IsSuccess = false;
                    }
                    else
                    {
                        var ex = APIExceptionHelper.CreateExceptionFromResponseErrors(response);
                        result.Error = ex.Message;
                        result.IsSuccess = false;
                    }

                    result.StatusCode = response.StatusCode;
                }
            }
            catch
            {
                result.IsSuccess = false;
                result.Error = "An error has occured please contact the administrator.";
            }

            return result;
        }

        /// <summary>
        /// PUT API call
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="param"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<ServiceModel<TReturn>> PutAPI(Uri uri, string param, string refreshToken = null)
        {
            var result = new ServiceModel<TReturn>();
            var paramcontent = new StringContent(param, Encoding.UTF8, "application/json");

            try
            {
                using (var _client = new HttpClient())
                {
                    var token = SetToken(refreshToken);

                    if (token != null)
                    {
                        _client.DefaultRequestHeaders.Authorization = token;
                    }

                    var response = await _client.PutAsync(uri, paramcontent);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        result.Data = JsonConvert.DeserializeObject<TReturn>(content);
                        result.IsSuccess = true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        result.IsSuccess = false;
                    }
                    else
                    {
                        var ex = APIExceptionHelper.CreateExceptionFromResponseErrors(response);
                        result.Error = ex.Message;
                        result.IsSuccess = false;
                    }

                    result.StatusCode = response.StatusCode;
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Error = "An error has occured please contact the administrator.";
            }

            return result;
        }

        /// <summary>
        /// DELETE API call
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private async Task<ServiceModel<TReturn>> DeleteAPI(Uri uri, string refreshToken = null)
        {
            var result = new ServiceModel<TReturn>();

            try
            {
                using (var _client = new HttpClient())
                {
                    var token = SetToken(refreshToken);

                    if (token != null)
                    {
                        _client.DefaultRequestHeaders.Authorization = token;
                    }

                    var response = await _client.DeleteAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        result.Data = JsonConvert.DeserializeObject<TReturn>(content);
                        result.IsSuccess = true;
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        result.IsSuccess = false;
                    }
                    else
                    {
                        var ex = APIExceptionHelper.CreateExceptionFromResponseErrors(response);
                        result.Error = ex.Message;
                        result.IsSuccess = false;
                    }

                    result.StatusCode = response.StatusCode;
                }
            }
            catch (Exception)
            {
                result.IsSuccess = false;
                result.Error = "An error has occured please contact the administrator.";
            }

            return result;
        }


        /// <summary>
        /// SET TOKEN
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        private AuthenticationHeaderValue SetToken(string refreshToken = null)
        {
            try
            {
                var token = string.Empty;               

                return new AuthenticationHeaderValue("Bearer", token);
            }
            catch
            {
                // No token set here..
                return null;
            }
        }

      
        #endregion
    }

    public class ServiceModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }
    }
}
