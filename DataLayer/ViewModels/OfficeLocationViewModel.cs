using System.ComponentModel.DataAnnotations;

namespace DataLayer.ViewModels
{
    public class OfficeLocationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the location name")]
        [Display(Name = "Location Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the location address")]
        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}
