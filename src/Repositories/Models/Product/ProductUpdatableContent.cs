namespace Repositories.Models.Product;

public class ProductUpdatableContent
{
    public string? Notes { get; set; }
    public required string Title { get; set; }
    public required string Type { get; set; }
    public decimal? Price { get; set; }
}