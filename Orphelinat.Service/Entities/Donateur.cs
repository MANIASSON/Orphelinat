//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Orphelinat.Service.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Donateur
    {
        public int IdDonateur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Telephone { get; set; }
        public string NatureDon { get; set; }
        public int IdOrphelinat { get; set; }
    
        public virtual Orphelinat Orphelinat { get; set; }
    }
}