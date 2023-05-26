namespace WebClient.Models;

public class UpdateOrder
{
    public DateTime? OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public decimal? Total { get; set; }

    public string? OrderStatus { get; set; }
}
