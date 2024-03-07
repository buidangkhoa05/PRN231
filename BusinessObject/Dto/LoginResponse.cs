using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string  Fullname { get; set; }
        public string Role { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
