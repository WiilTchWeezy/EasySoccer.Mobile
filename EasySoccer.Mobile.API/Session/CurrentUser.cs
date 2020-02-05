using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        public Guid? UserId
        {
            get
            {
                var claimValue = GetClaimByType("UserId");
                if (string.IsNullOrEmpty(claimValue) == false)
                {
                    Guid userId;
                    if (Guid.TryParse(claimValue, out userId))
                        return userId;
                    return null;
                }
                return null;
            }
        }

        public void LogOff()
        {
            Preferences.Remove("AuthToken");
            Preferences.Remove("AuthExpiresDate");
        }

        public List<Claim> DecryptToken()
        {
            if (string.IsNullOrEmpty(AuthToken) == false)
            {
                var jwtToken = new JwtSecurityToken(AuthToken);
                if (jwtToken != null && jwtToken.Claims != null && jwtToken.Claims.ToList().Count > 0)
                {
                    return jwtToken.Claims.ToList();
                }
            }
            return new List<Claim>();
        }

        public string GetClaimByType(string type)
        {
            var claims = DecryptToken();
            if (claims.Count > 0)
            {
                return claims.Where(x => x.Type == type).Select(x => x.Value).FirstOrDefault();
            }
            return String.Empty;
        }
    }
}
