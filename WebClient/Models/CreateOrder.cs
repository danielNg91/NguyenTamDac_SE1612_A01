using System.ComponentModel.DataAnnotations;

namespace WebClient.Models;

public class CreateOrder
{
    [Required, Range(0, int.MaxValue)]
    public int CustomerId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public string OrderStatus { get; set; }
    public decimal? Total { get; set; }

}
