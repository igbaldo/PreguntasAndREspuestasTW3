using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TPWebIII.Models.WrapperEntities
{
    public class LoginWrapper
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Profesor { get; set; }
        public string ReturnUrl { get; set; }
    }
}