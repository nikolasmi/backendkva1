using System;
using System.Collections.Generic;

namespace projekatKVA.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Size { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public DateOnly DateAdded { get; set; }

    public double Price { get; set; }

    public string? PicturePath { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Wish> Wishes { get; set; } = new List<Wish>();
}
