using System;

namespace EasySoccer.Mobile.API.Infra.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public ApiException(string message) : base(message)
        {

        }
    }
}
