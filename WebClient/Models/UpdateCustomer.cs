using System.ComponentModel.DataAnnotations;

namespace WebClient.Models;


public class UpdateCustomer
{
    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; }

    [MaxLength(180)]
    public string? CustomerName { get; set; }

    [MaxLength(15)]
    public string? City { get; set; }

    [MaxLength(15)]
    public string? Country { get; set; }

    [MaxLength(30)]
    public string? Password { get; set; }

    public DateTime? Birthday { get; set; }
}
