using Common.ResultPattern;
using Repositories.Models.Product;

namespace Repositories.Abstract;

public interface IProductRepository
{
    Task<IEnumerable<ProductInformation>> GetProductsAsync(ProductQuery query);
    Task<Result<ProductDetail>> GetProductById(string id);
    Task<Result> AddProductAsync(ProductContent content);
    Task<Result> UpdateProductAsync(string id ,ProductUpdatableContent content);
    Task<Result> DeleteProductAsync(string id);
}