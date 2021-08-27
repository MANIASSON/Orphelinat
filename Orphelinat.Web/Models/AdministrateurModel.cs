using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orphelinat.Web.Models
{
    public class AdministrateurModel
    {
        public int IdAdministrateur { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
    }
}