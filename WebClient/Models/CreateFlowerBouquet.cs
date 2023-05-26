using System.ComponentModel.DataAnnotations;

namespace WebClient.Models;

public class CreateFlowerBouquet
{
    [Required]
    [Range(1, int.MaxValue)]
    public int FlowerBouquetId { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public string FlowerBouquetName { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Range(0, (double)decimal.MaxValue)]
    public decimal UnitPrice { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int UnitsInStock { get; set; }

    public byte? FlowerBouquetStatus { get; set; }
    public int? SupplierId { get; set; }

}
