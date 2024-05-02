using System.ComponentModel.DataAnnotations;

namespace DataLayer.ViewModels
{
    public class OrderViewModel
    {
        public int DrinkId { get; set; }
        public int Quantity { get; set; } = 1;
        [Required]
        public int OfficeId { get; set; }

        public string Id { get; set; }
        public bool IsEmployeeOrder { get; set; }
    }
}
