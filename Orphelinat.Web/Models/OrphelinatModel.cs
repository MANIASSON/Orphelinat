using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Orphelinat.Web.Models
{
    public class OrphelinatModel
    {
        public int IdOrphelinat { get; set; }
        public string Nom { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public string Telephone { get; set; }
        public string Adresse { get; set; }
        public string Devise { get; set; }
        public System.DateTime DateCreation { get; set; }
        public string Image { get; set; }
        [Compare(nameof(NCMotDePasse))]
        public string NCMotDePasse { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }

        [JsonIgnore]
        public HttpPostedFileBase File { get; set; }

        public virtual ICollection<BesoinModel> Besoins { get; set; }
        public virtual ICollection<DonateurModel> Donateurs { get; set; }
        public virtual ICollection<EvenementModel> Evenements { get; set; }
    }
}