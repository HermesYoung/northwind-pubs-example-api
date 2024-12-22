namespace NorthwindPubsApi.Controllers.Product.Models;

public class ProductUpdateRequest
{
    public string? Notes { get; set; }
    public required string Title { get; set; }
    public required string Type { get; set; }
    public decimal? Price { get; set; }
}