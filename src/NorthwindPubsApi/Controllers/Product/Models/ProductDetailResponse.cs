namespace NorthwindPubsApi.Controllers.Product.Models;

public class ProductDetailResponse
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Type { get; set; }
    public decimal? Price { get; set; }
    public DateTime PublishDate { get; set; }
    public string? Publisher { get; set; }
    public string? Notes { get; set; }
    public string? Author { get; set; }
}