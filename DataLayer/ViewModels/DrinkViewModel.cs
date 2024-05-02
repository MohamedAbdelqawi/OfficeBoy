using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.ViewModels
{
    public class DrinkViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Price field is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The Price field must be greater than 0.")]
        public decimal Price { get; set; }


        public string? PictureUrl { get; set; }

        [Required(ErrorMessage = "The Availability field is required.")]
        public bool Availability { get; set; }

        [Required(ErrorMessage = "The Time To Prepare Minutes field is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "The Time To Prepare Minutes field must be greater than 0.")]
        public int TimeToPrepareMinutes { get; set; }


        public IFormFile? PictureFile { get; set; }
    }
}
