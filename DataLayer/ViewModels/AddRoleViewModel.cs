using System.ComponentModel.DataAnnotations;

namespace DataLayer.ViewModels
{
    public class AddRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }

}
