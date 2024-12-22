namespace NorthwindPubsApi.Controllers.Product.Models;

public class ProductQueryResponse
{
    public required int Page { get; set; }
    public required int Count { get; set; }
    public required IEnumerable<ProductView> Products { get; set; }

    public class ProductView
    {
        public required string Title { get; set; }
        public required string Type { get; set; }
        public decimal? Price { get; set; }
        public DateTime PublishDate { get; set; }
        public string? Publisher { get; set; }
        public required string Id { get; set; }
    }
}