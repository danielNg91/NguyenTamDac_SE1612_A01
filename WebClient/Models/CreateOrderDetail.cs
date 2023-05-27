using System.ComponentModel.DataAnnotations;

namespace WebClient.Models;

public class CreateOrderDetail
{
    [Required]
    public int OrderId { get; set; }
    
    [Required]
    public int FlowerBouquetId { get; set; }
    
    [Required]
    public decimal UnitPrice { get; set; }
    
    [Required, Range(1, int.MaxValue)]

    public int Quantity { get; set; }

    [Required, Range(0, 100)]
    public double Discount { get; set; }
}
