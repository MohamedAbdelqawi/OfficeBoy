using Microsoft.AspNetCore.Identity;

namespace DataLayer.Entities
{
    public class Employee : IdentityUser
    {


        public string Name { get; set; }
        public string EmpId { get; set; }
        public string PhoneNumber { get; set; }
        public string OfficeLocation { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; }

    }
}
