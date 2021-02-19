using Shared;
using System;

namespace Domain.Entities
{
    public class Depense : BaseEntity
    {
        public int? UserId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
        public NatureDepense NatureDepense { get; set; }
        public double Montant { get; set; }
        public int? DeviseId { get; set; }
        public Devise Devise { get; set; }
        public string Commentaire { get; set; }
    }
}
