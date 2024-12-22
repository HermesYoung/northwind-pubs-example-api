namespace Repositories.Models.Product;

public class ProductInformation
{
    public required string Title { get; set; }
    public required string Type { get; set; }
    public decimal? Price { get; set; }
    public DateTime PublishDate { get; set; }
    public PublisherInformation? Publisher { get; set; }
    public required string Id { get; set; }

    public class PublisherInformation
    {
        public required string Id { get; set; }
        public string? Name { get; set; }
    }
}