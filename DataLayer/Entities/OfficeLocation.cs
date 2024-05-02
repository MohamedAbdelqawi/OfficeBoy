namespace DataLayer.Entities
{
    public class OfficeLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; }
    }
}
