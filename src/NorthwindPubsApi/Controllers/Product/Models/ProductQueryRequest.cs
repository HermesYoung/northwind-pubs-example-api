namespace NorthwindPubsApi.Controllers.Product.Models;

public class ProductQueryRequest
{
    public required int PageSize { get; set; }
    public required int PageNumber { get; set; }
    public string? Title { get; set; }
}