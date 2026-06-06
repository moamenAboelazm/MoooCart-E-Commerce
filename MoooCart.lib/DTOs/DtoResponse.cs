using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoooCart.lib.DTOs
{
    public class DtoResponse
    {
        public DtoResponse(bool _success = false, string _messege = "")
        {
            Success = _success;
            msg = _messege;
        }

       public bool Success { get; set; }
       public string msg { get; set; }
    }

    public class DtoLoginResponse
    {
        public DtoLoginResponse(bool _success = false, string _messege = "", string token = "", string refreshToken = "")
        {
            Success = _success;
            msg = _messege;
            Token = token;
            RefreshToken = refreshToken;
        }

        public bool Success { get; set; }
        public string msg { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }

    }
}
