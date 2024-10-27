using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models.User
{
    public class UserLoginResponse
    {
        public int Status { get; set; }
        public string access_token { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
