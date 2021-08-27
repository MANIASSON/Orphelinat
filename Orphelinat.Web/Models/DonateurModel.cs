using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orphelinat.Web.Models
{
    public class DonateurModel
    {
        public int IdDonateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Telephone { get; set; }
        public string NatureDon { get; set; }
        public int IdOrphelinat { get; set; }

        public virtual OrphelinatModel Orphelinat { get; set; }
    }
}