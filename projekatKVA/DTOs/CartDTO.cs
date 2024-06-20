using projekatKVA.Models;

namespace projekatKVA.DTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }

        public int ItemId { get; set; }

        public int UserId { get; set; }

        public string ItemName { get; set; }

        public string ItemPicturePath { get; set; }

        public int ItemPrice { get; set; }

        public int Quantity { get; set; }

        //public virtual Item Item { get; set; } = null!;

        //public virtual User User { get; set; } = null!;
    }
}
