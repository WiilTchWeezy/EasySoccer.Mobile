using Newtonsoft.Json;
using System;

namespace EasySoccer.Mobile.API.Infra.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public ApiException()
        {

        }

        public ApiException(string message) : base(message)
        {

        }
    }
}
