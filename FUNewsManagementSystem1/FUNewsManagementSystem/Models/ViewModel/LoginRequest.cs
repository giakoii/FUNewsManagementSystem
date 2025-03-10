using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Models.ViewModel
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
    }
}
