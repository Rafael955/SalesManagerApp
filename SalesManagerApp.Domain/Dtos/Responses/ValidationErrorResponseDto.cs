namespace SalesManagerApp.Domain.Dtos.Responses
{
    public class ValidationErrorResponseDto
    {
        public string? PropertyName { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
