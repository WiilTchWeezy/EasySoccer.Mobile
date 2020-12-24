using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.Mobile.API.ApiResponses
{
    public class InserTokenResponse
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public long? CompanyUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LogOffDate { get; set; }
    }
}
