﻿namespace EFCore7Demo.Models;

public class Address { 
    public int Id { get; set; }
    public string StreetNumber { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }

    public int PersonId { get; set; }
}
