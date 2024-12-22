namespace Repositories.Models.Product;

public class ProductContent
{
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Type { get; set; }
    public decimal? Price { get; set; }
    public decimal? Advance { get; set; }
    public int? Royalty { get; set; }
    public string? Notes { get; set; }
    public string? PublisherId { get; set; }
    public ICollection<string>? AuthorIds { get; set; }
}