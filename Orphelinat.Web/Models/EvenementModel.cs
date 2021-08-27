using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orphelinat.Web.Models
{
    public class EvenementModel
    {
        public int IdEvenement { get; set; }
        public string Libele { get; set; }
        public string Description { get; set; }
        public System.DateTime DateDebut { get; set; }
        public System.DateTime DateFin { get; set; }
        public int IdOrphelinat { get; set; }

        public virtual OrphelinatModel Orphelinat { get; set; }
    }
}