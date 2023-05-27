namespace WebClient.Models;

public class UpdateOrder
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public DateTime? OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public decimal? Total { get; set; }

    public string? OrderStatus { get; set; }
}
