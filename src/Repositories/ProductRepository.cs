using Common.ResultPattern;
using DatabaseContext.Context;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstract;
using Repositories.Models.Product;

namespace Repositories;

internal class ProductRepository(NorthwindPubsDbContext context) : IProductRepository
{
    public async Task<IEnumerable<ProductInformation>> GetProductsAsync(ProductQuery query)
    {
        var baseQuery = context.Titles.Include(x => x.Pub).AsQueryable();

        if (!string.IsNullOrEmpty(query.TitleStartWith))
        {
            baseQuery = baseQuery.Where(x => x.Title1.StartsWith(query.TitleStartWith));
        }

        baseQuery = baseQuery.OrderBy(x => x.Title1).Skip(query.PageNumber * query.PageSize).Take(query.PageSize);
        var products = await baseQuery.ToListAsync();
        return products.Select(x => new ProductInformation
        {
            Title = x.Title1,
            Type = x.Type,
            Price = x.Price,
            PublishDate = x.Pubdate,
            Publisher = x.Pub is null
                ? null
                : new ProductInformation.PublisherInformation()
                {
                    Id = x.Pub.PubId,
                    Name = x.Pub.PubName,
                },
        });
    }

    public async Task<Result<ProductDetail>> GetProductById(string id)
    {
        var result =  await context.Titleauthors
            .Include(x => x.Title)
            .ThenInclude(x => x.Pub)
            .Include(x => x.Au)
            .FirstOrDefaultAsync(x => x.Title.TitleId == id);
        if (result == null)
        {
            return Result.Failure<ProductDetail>(new DefaultErrorMessage(404, "Title not found"));
        }

        return Result.Success(new ProductDetail
        {
            Title = result.Title.Title1,
            Type = result.Title.Type,
            Price = result.Title.Price,
            PublishDate = result.Title.Pubdate,
            Publisher = result.Title.Pub is null
                ? null
                : new ProductInformation.PublisherInformation()
                {
                    Id = result.Title.Pub.PubId,
                    Name = result.Title.Pub.PubName,
                },
            Notes = result.Title.Notes,
            Author = $"{result.Au.AuFname} {result.Au.AuLname}",
        });
    }
}