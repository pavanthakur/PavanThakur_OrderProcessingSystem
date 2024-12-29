namespace IODataLabs.OrderProcessingSystem.Application.DTO
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<OrderProductDto> OrderProductDtos { get; set; }
    }
}
