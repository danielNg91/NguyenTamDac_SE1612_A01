using System.ComponentModel.DataAnnotations;

namespace WebClient.Models;

public class UpdateFlowerBouquet
{
    public int? CategoryId { get; set; }
    public string? FlowerBouquetName { get; set; }

    public string? Description { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    public decimal? UnitPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int? UnitsInStock { get; set; }

    [Range(0, 255)]
    public byte? FlowerBouquetStatus { get; set; }
    public int? SupplierId { get; set; }

}
