using EasySoccer.Mobile.API.Infra.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EasySoccer.Mobile.API
{
    public class ApiClient
    {
        private static ApiClient _apiClient;
        public static ApiClient Instance
        {
            get
            {
                if (_apiClient == null)
                {
                    _apiClient = new ApiClient();
                }
                return _apiClient;
            }
        }

        const string ApiUrl = "";

        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ApiUrl);
            httpClient.DefaultRequestHeaders.Clear();
            return httpClient;
        }

        private async Task<T> TreatApiReturn<T>(HttpResponseMessage httpResponse)
        {
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync());
            else
                throw new ApiException(await httpResponse.Content.ReadAsStringAsync());
        }

        private async Task<T> Get<T>(string apiMethod)
        {
            using (var httpClient = CreateClient())
            {
                return await TreatApiReturn<T>(await httpClient.GetAsync(apiMethod));
            }
        }

        private async Task<TReturn> Post<TReturn, TRequest>(string apiMethod, TRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                return await TreatApiReturn<TReturn>(await httpClient.PostAsJsonAsync(apiMethod, request));
            }
        }

        public async Task LoginAsync()
        {

        }
    }
}
