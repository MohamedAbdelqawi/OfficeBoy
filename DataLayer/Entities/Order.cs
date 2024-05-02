using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class Order
    {

        public int Id { get; set; }

        public int Quantity { get; set; }

        public bool IsEmployeeOrder { get; set; }
        public DateTime DateTimeOfOrder { get; set; } = DateTime.Now;

        public OrderStatus Status { get; set; }

        // Navigation properties

        [ForeignKey("Employee")]
        public string EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        [ForeignKey("Drink")]
        public int DrinkId { get; set; }

        public virtual Drink Drink { get; set; }
        [ForeignKey("OfficeLocation")]
        public int OfficeId { get; set; }
        public virtual OfficeLocation OfficeLocation { get; set; }

    }
}
