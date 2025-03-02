using System;
using System.Collections.Generic;

namespace MovieRental.Models;

public partial class Rental
{
    public int Id { get; set; }

    public int? MovieId { get; set; }

    public int? CustomerId { get; set; }

    public DateOnly? RentalDate { get; set; }

    public DateOnly? ReturnDate { get; set; }
}
