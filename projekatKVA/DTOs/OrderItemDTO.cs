namespace projekatKVA.DTOs
{
    public class OrderItemDTO
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ItemId { get; set; }

        public string ItemName { get; set; } = null!;

        public int ItemPrice { get; set; }

        public int Quantity { get; set; }

        public DateOnly OrderDate { get; set; }
    }
}
