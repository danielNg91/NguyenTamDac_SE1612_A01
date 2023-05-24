using Api.Mappings;
using System.ComponentModel.DataAnnotations;
using Repository.Models;

namespace Api.Models;

public class CreateFlowerBouquet : IMapTo<FlowerBouquet>
{
    [Required]
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
