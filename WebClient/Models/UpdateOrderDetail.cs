using BusinessObjects;

namespace WebClient.Models;

public class UpdateOrderDetail
{
    public int OrderId { get; set; }
    public int? FlowerBouquetId { get; set; }
    public decimal? UnitPrice { get; set; }
    public int? Quantity { get; set; }
    public double? Discount { get; set; }
    public FlowerBouquet FlowerBouquet { get; set; }
}
