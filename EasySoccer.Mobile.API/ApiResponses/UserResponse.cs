using System;
using System.Collections.Generic;
using System.Text;

namespace EasySoccer.Mobile.API.ApiResponses
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}
