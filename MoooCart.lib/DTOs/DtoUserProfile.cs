using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.DTOs
{
    public class DtoUserProfile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ProfileImgUrl { get; set; }
        public bool IsActive { get; set; }
    }

    public class DtoUpdateProfile
    {
        public string FullName { get; set; }
        public string ProfileImgUrl { get; set; }
    }

    public class DtoChangePassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
