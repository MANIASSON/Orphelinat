using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orphelinat.Web.Models
{
    public class ConnexionModel
    {
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
    }
}