using Shared;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? DeviseId { get; set; }
        public Devise Devise { get; set; }
        public List<Depense> Depenses { get; set; }

    }
}
