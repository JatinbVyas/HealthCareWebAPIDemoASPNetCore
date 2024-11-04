using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthCareFrontEnd.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DisplayName("Name")]
        [MaxLength(20)]
        public string NameofUser { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
