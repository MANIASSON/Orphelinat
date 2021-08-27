using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Orphelinat.Web.Models
{
    public class BesoinModel
    {
        public int IdBesoin { get; set; }
        public string Libele { get; set; }
        public int IdOrphelinat { get; set; }

        public virtual OrphelinatModel Orphelinat { get; set; }
    }
}