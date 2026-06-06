using MoooCart.id.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.id.Authentication.Base
{
    public interface IRoleManagement
    {
        Task<string?> GetUserRole(string UserEmail);
        Task<bool> AddUserToRole(AppUser user , string rolename);
    }
}
