using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.DTOs
{
    public class DtoBaseUser
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    
    public class DtoCreateUser : DtoBaseUser
    {
        public required string FullName { get; set; }
        public required string ConfirmPassword { get; set; }
    }

    public class DtoLoginUser : DtoBaseUser
    {
        
    }

}
