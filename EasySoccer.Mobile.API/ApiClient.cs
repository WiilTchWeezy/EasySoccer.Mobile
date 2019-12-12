﻿using Acr.UserDialogs;
using EasySoccer.Mobile.API.ApiResponses;
using EasySoccer.Mobile.API.Infra.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

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

        const string ApiUrl = "https://apieasysoccer.azurewebsites.net/api/";

        private HttpClient CreateClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ApiUrl);
            httpClient.DefaultRequestHeaders.Clear();
            if (Preferences.ContainsKey("AuthToken"))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + Preferences.Get("AuthToken", String.Empty));
            }
            return httpClient;
        }

        private async Task<T> TreatApiReturn<T>(HttpResponseMessage httpResponse)
        {
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync());
            else
            {
                UserDialogs.Instance.HideLoading();
                var response = await httpResponse.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(response))
                    throw new ApiException("Ops! Ocorreu um erro.");
                throw JsonConvert.DeserializeObject<ApiException>(response);
            }
        }

        private async Task<T> Get<T>(string apiMethod)
        {
            T response;
            UserDialogs.Instance.ShowLoading("");
            using (var httpClient = CreateClient())
            {
                response = await TreatApiReturn<T>(await httpClient.GetAsync(ApiUrl + apiMethod));
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

        private string GenerateQueryParameters(object parameters)
        {
            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in parameters.GetType().GetProperties())
            {
                queryString.Add(item.Name, item.GetValue(parameters).ToString());
            }
            return queryString.ToString();
        }

        private void SetUserPreferences(string token, DateTime expireDate)
        {
            Preferences.Remove("AuthToken");
            Preferences.Remove("AuthExpiresDate");
            Preferences.Set("AuthToken", token);
            Preferences.Set("AuthExpiresDate", expireDate);
        }

        public async Task<TokenResponse> LoginAsync(string email, string password)
        {
            var response = await Get<TokenResponse>("login/token?" + GenerateQueryParameters(new { email, password }));
            SetUserPreferences(response.Token, response.ExpireDate);
            return response;
        }

        public async Task<TokenResponse> LoginFromFacebook(string Email, string First_name, string Last_name, string Birthday, string Id)
        {
            var response = await Get<TokenResponse>("login/tokenfromfacebook?" + GenerateQueryParameters(new { Email, First_name, Last_name, Birthday, Id }));
            SetUserPreferences(response.Token, response.ExpireDate);
            return response;
        }

        public async Task<List<CompanyResponse>> GetCompaniesAsync()
        {
            var response = await Get<List<CompanyResponse>>("company/get");
            return response;
        }
    }
}
