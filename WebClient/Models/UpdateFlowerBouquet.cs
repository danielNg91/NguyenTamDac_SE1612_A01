using System.ComponentModel.DataAnnotations;

namespace WebClient.Models;

public class UpdateFlowerBouquet
{
    public string? FlowerBouquetName { get; set; }

    public string? Description { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    public decimal? UnitPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int? UnitsInStock { get; set; }

    public byte? FlowerBouquetStatus { get; set; }
    public int? SupplierId { get; set; }

}
