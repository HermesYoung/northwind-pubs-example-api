using Common.ResultPattern;
using Repositories.Models.Product;

namespace Repositories.Abstract;

public interface IProductRepository
{
    Task<IEnumerable<ProductInformation>> GetProductsAsync(ProductQuery query);
    Task<Result<ProductDetail>> GetProductById(string id);
}