using Api.Mappings;
using Api.Utils;
using BusinessObjects;

namespace Api.Models;

public class LoginResponse : IMapFrom<Customer>
{
    public int CustomerId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
}
