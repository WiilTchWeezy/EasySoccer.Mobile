using System;

namespace EasySoccer.Mobile.API.ApiResponses
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
