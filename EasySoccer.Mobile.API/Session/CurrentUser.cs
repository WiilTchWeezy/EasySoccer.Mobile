using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace EasySoccer.Mobile.API.Session
{
    public class CurrentUser
    {
        private static CurrentUser _currentUser;
        public static CurrentUser Instance
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = new CurrentUser();
                return _currentUser;
            }
        }

        public DateTime? AuthExpiresDate
        {
            get
            {
                if (Preferences.ContainsKey("AuthExpiresDate"))
                    return Preferences.Get("AuthExpiresDate", DateTime.MinValue);
                else
                    return null;
            }
        }

        public string AuthToken
        {
            get
            {
                if (Preferences.ContainsKey("AuthToken"))
                    return Preferences.Get("AuthToken", String.Empty);
                else
                    return null;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return AuthExpiresDate.HasValue && AuthExpiresDate.Value > DateTime.Now && string.IsNullOrEmpty(AuthToken) == false;
            }
        }

        public void LogOff()
        {
            Preferences.Remove("AuthToken");
            Preferences.Remove("AuthExpiresDate");
        }
    }
}
