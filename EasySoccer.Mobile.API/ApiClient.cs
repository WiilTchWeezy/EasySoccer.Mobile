using Acr.UserDialogs;
using EasySoccer.Mobile.API.ApiResponses;
using EasySoccer.Mobile.API.Infra.Exceptions;
using EasySoccer.Mobile.API.Session;
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
            try
            {
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    var tConfig = new ToastConfig("Você não esta conectado. Verifique sua conexão com internet.")
                    {
                        MessageTextColor = System.Drawing.Color.Yellow,
                        Duration = TimeSpan.FromSeconds(30)
                    };
                    UserDialogs.Instance.Toast(tConfig);
                }
                var httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(ApiUrl);
                httpClient.DefaultRequestHeaders.Clear();
                if (Preferences.ContainsKey("AuthToken"))
                {
                    var token = Preferences.Get("AuthToken", String.Empty);
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                return httpClient;
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                throw;
            }
        }

        private async Task<T> TreatApiReturn<T>(HttpResponseMessage httpResponse)
        {
            try
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<T>(await httpResponse.Content.ReadAsStringAsync());
                else if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    UserDialogs.Instance.HideLoading();
                    CurrentUser.Instance.LogOff();
                    throw new ApiUnauthorizedException("Ops! Você não está mais autenticado.");
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    var response = await httpResponse.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(response))
                        throw new ApiException("Ops! Ocorreu um erro.");
                    var error = JsonConvert.DeserializeObject<ApiError>(response);
                    var apiException = new ApiException(error.message);
                    throw apiException;
                }
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                throw;
            }
        }

        private async Task<T> Get<T>(string apiMethod)
        {
            try
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
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                throw;
            }
        }

        private async Task<TReturn> Post<TReturn>(string apiMethod, object request)
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
            var location = await Geolocation.GetLastKnownLocationAsync();
            string parameters = String.Empty;
            if (location != null)
                parameters = GenerateQueryParameters(new { location?.Longitude, location?.Latitude });
            var response = await Get<List<CompanyResponse>>("company/get?" + parameters);
            return response;
        }

        public async Task<List<SoccerPitchResponse>> GetSoccerPitchesAsync(long companyId)
        {
            return await Get<List<SoccerPitchResponse>>("soccerpitch/getbycompanyid?" + GenerateQueryParameters(new { Page = 1, PageSize = 99, CompanyId = companyId }));
        }

        public async Task<List<SportTypeResponse>> GetSportTypesAsync(long companyId)
        {
            return await Get<List<SportTypeResponse>>("soccerpitch/getsporttypes?" + GenerateQueryParameters(new { companyId = companyId }));
        }

        public async Task<List<AvaliableSchedulesResponse>> GetAvaliableSchedulesAsync(long companyId, string selectedDate, int sportType)
        {
            return await Get<List<AvaliableSchedulesResponse>>("SoccerPitchReservation/getavaliableschedules?" + GenerateQueryParameters(new { CompanyId = companyId, SelectedDate = selectedDate, SportType = sportType }));
        }

        public async Task<List<SoccerPitchPlanResponse>> GetSoccerPitchPlanBySoccerPitchAsync(long soccerPitchId)
        {
            return await Get<List<SoccerPitchPlanResponse>>("SoccerPitchPlan/getbysoccerpitch?" + GenerateQueryParameters(new { SoccerPitchId = soccerPitchId }));
        }

        public async Task<SoccerPitchReservationResponse> MakeReservationAsync(long soccerPitchId, Guid userId, DateTime selectedDate, TimeSpan hourStart, TimeSpan hourEnd, int soccerPitchPlanId)
        {
            return await Post<SoccerPitchReservationResponse>("SoccerPitchReservation/makeschedule", new
            {
                SoccerPitchId = soccerPitchId,
                UserId = userId,
                SelectedDate = selectedDate,
                HourStart = hourStart,
                HourEnd = hourEnd,
                SoccerPitchSoccerPitchPlanId = soccerPitchPlanId
            });
        }

        public async Task<UserResponse> CreateUserAsync(string name, string phoneNumber, string socialMedia, string email, string password)
        {
            return await Post<UserResponse>("User/create", new
            {
                Name = name,
                PhoneNumber = phoneNumber,
                SocialMedia = socialMedia,
                Email = email,
                Password = password
            });
        }

        public async Task<UserResponse> GetUserInfoAsync()
        {
            return await Get<UserResponse>("user/getInfo");
        }

        public async Task<UserResponse> UpdateUserInfoAsync(string name, string phoneNumber, string email)
        {
            return await Post<UserResponse>("User/patch", new
            {
                Name = name,
                PhoneNumber = phoneNumber,
                Email = email
            });
        }

        public async Task<List<SoccerPitchReservationResponse>> GetUserSchedulesAsync()
        {
            return await Get<List<SoccerPitchReservationResponse>>("SoccerPitchReservation/getuserschedules");
        }

        public async Task<CompanySchedulesResponse> GetCompanySchedulesAsync(int companyId, int dayOfWeek)
        {
            return await Get<CompanySchedulesResponse>("CompanySchedule/get?" + GenerateQueryParameters(new { companyId, dayOfWeek }));
        }

    }
}
