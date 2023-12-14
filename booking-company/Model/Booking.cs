using System;
using System.Collections.Generic;

namespace booking_company.Model;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? ClientId { get; set; }

    public int? HouseId { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string? Status { get; set; }

    public virtual Client? Client { get; set; }

    public virtual House? House { get; set; }
}
