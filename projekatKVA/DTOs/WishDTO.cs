namespace projekatKVA.DTOs
{
    public class WishDTO
    {
        public int WishId { get; set; }

        public int ItemId { get; set; }

        public int UserId { get; set; }

        public string ItemName { get; set; }

        public string ItemPicturePath { get; set; }

        public int ItemPrice { get; set; }
    }
}
