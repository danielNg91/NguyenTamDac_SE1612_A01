using Api.Mappings;
using Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class UpdateFlowerBouquet : IMapTo<FlowerBouquet>
{
    public int? CategoryId { get; set; }

    public string? FlowerBouquetName { get; set; }

    public string? Description { get; set; }

    [Range(0, (double)decimal.MaxValue)]
    public decimal? UnitPrice { get; set; }

    [Range(0, int.MaxValue)]
    public int? UnitsInStock { get; set; }

    public byte? FlowerBouquetStatus { get; set; }
    public int? SupplierId { get; set; }

}
