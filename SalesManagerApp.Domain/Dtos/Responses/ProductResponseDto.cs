namespace SalesManagerApp.Domain.Dtos.Responses
{
    public class ProductResponseDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
