using System;
using System.Collections.Generic;

namespace MovieRental.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? MembershipDate { get; set; }
}
