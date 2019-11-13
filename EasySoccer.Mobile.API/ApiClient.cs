using Acr.UserDialogs;
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
            {
                UserDialogs.Instance.HideLoading();
                throw JsonConvert.DeserializeObject<ApiException>(await httpResponse.Content.ReadAsStringAsync());
            }
        }

        private async Task<T> Get<T>(string apiMethod)
        {
            T response;
            UserDialogs.Instance.ShowLoading("");
            using (var httpClient = CreateClient())
            {
                response = await TreatApiReturn<T>(await httpClient.GetAsync(apiMethod));
            }
            UserDialogs.Instance.HideLoading();
            return response;
        }

        private async Task<TReturn> Post<TReturn, TRequest>(string apiMethod, TRequest request)
        {
            TReturn response;
            UserDialogs.Instance.ShowLoading("");
            using (var httpClient = CreateClient())
            {
                response = await TreatApiReturn<TReturn>(await httpClient.PostAsJsonAsync(apiMethod, request));
            }
            UserDialogs.Instance.HideLoading();
            return response;
        }

        public async Task LoginAsync()
        {

        }
    }
}
