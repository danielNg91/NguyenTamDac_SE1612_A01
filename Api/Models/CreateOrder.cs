﻿using Api.Mappings;
using BusinessObjects;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class CreateOrder : IMapTo<Order>
{
    [Required]
    [Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    public CartFlower[] Flowers { get; set; }

    public DateTime? ShippedDate { get; set; }
    
    public string OrderStatus { get; set; }

}
