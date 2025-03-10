using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Models.ViewModel;

public class UserProfileViewModel
{
    public string? Email { get; set; }
    
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Name must contain only letters")]
    public string? Name { get; set; }
}