using Api.Mappings;
using Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class CreateOrder : IMapTo<Order>
{
    [Required]
    [Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }
    
    public DateTime? ShippedDate { get; set; }
    
    public decimal? Total { get; set; }
    
    public string OrderStatus { get; set; }
}
