using System;
using System.Collections.Generic;

namespace projekatKVA.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int ItemId { get; set; }

    public int UserId { get; set; }

    public string Status { get; set; } = null!;

    public double Rating { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
