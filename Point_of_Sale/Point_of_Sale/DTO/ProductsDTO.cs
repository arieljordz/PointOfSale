namespace Point_of_Sale.DTO
{
    public class ProductsDTO
    {
        public int ProductId { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Supplier { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int InvoiceId { get; set; }
        public decimal SubTotal { get; set; }
        public string? DateAdded { get; set; }
        public string? DateExpired { get; set; }
    }

    public class ProductsDTOList : List<ProductsDTO> { }
}
