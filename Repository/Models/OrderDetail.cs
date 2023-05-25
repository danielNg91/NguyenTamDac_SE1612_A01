using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace Repository.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int FlowerBouquetId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }

        public virtual FlowerBouquet FlowerBouquet { get; set; }
        [JsonIgnore]
        public virtual Order Order { get; set; }
    }
}
