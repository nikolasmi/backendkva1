namespace projekatKVA.DTOs
{
    public class ItemDTO
    {
        public int ItemId { get; set; }

        public string Name { get; set; } = null!;

        public string Type { get; set; } = null!;

        public string Size { get; set; } = null!;

        public string Manufacturer { get; set; } = null!;

        public DateOnly DateAdded { get; set; }

        public double Price { get; set; }

        public string? PicturePath { get; set; }
    }
}
