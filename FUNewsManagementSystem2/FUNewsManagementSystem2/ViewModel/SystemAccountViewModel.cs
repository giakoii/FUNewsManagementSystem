using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem2.ViewModel;

public class SystemAccountViewModel
{
    public short AccountId { get; set; }
    
    [Required, EmailAddress]
    public string AccountEmail { get; set; }
    
    [Required]
    public string AccountName { get; set; }

    [Required]
    public int AccountRole { get; set; }
}