using App.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IJwtService
    {
        Task<string> Authenticate(int userId, string userName);
    }
}
