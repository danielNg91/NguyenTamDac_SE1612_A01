using Api.Mappings;
using Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class UpdateCustomer : IMapTo<Customer>
{
    public string CustomerName { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public string Password { get; set; }

    public DateTime? Birthday { get; set; }
}
