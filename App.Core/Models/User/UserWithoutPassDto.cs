using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models.User
{
    public class UserWithoutPassDto
    {
        public int UserId { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public bool? IsChecked { get; set; }
    }
}
