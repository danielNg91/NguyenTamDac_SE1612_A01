﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

#nullable disable

namespace BusinessObjects;

public partial class Customer
{
    public Customer()
    {
        Orders = new HashSet<Order>();
    }

    public int CustomerId { get; set; }
    public string Email { get; set; }
    public string CustomerName { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Password { get; set; }
    public DateTime? Birthday { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
