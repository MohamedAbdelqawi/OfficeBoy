namespace DataLayer.Entities
{
    public class Drink
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public bool Availability { get; set; }
        public int TimeToPrepareMinutes { get; set; }

        // Navigation property
        public virtual ICollection<Order> Orders { get; set; }

    }
}
