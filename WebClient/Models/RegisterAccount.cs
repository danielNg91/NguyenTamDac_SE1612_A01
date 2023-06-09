﻿using BusinessObjects;
using System.ComponentModel.DataAnnotations;

namespace WebClient.Models;
public class RegisterAccount
{
    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; }

    [Required, MaxLength(180)]
    public string CustomerName { get; set; }

    [Required, MaxLength(15)]
    public string City { get; set; }

    [Required, MaxLength(15)]
    public string Country { get; set; }

    [Required, MaxLength(30)]
    public string Password { get; set; }

    public DateTime? Birthday { get; set; }
}
