using System.ComponentModel.DataAnnotations;

namespace WebClient.Models;

public class CartFlower
{
    [Required]
    public int FlowerBouquetId { get; set; }

    [Required]
    public int Quantity { get; set; }
}
