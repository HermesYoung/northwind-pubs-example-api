namespace Repositories.Models.Product;

public class ProductQuery
{
    public string? TitleStartWith { get; set; }
    public required int PageSize { get; set; }
    public required int PageNumber { get; set; }
}