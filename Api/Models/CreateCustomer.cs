using Api.Mappings;
using Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class CreateCustomer : IMapTo<Customer>
{
    [Required]
    [Range(1, int.MaxValue)]
    public int CustomerId { get; set; }
    
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string CustomerName { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string Country { get; set; }

    [Required]
    public string Password { get; set; }

    public DateTime? Birthday { get; set; }
}
