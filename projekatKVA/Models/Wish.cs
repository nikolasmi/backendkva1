using System;
using System.Collections.Generic;

namespace projekatKVA.Models;

public partial class Wish
{
    public int WishId { get; set; }

    public int ItemId { get; set; }

    public int UserId { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
