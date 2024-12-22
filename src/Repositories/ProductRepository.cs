using System.Runtime.CompilerServices;
using Common.ResultPattern;
using DatabaseContext.Context;
using DatabaseContext.Entities;
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
            Id = x.TitleId,
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

    public async Task<Result<ProductDetail>> GetProductByIdAsync(string id)
    {
        var result = await context.Titleauthors
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
            Id = result.TitleId,
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

    public async Task<Result> AddProductAsync(ProductContent content)
    {
        var dateTime = DateTime.Now;
        var publisher = await context.Publishers.FindAsync(content.PublisherId);
        if (publisher == null)
        {
            return Result.Failure(new DefaultErrorMessage(401, "Invalid publisher"));
        }

        await context.Titles.AddAsync(new Title
        {
            TitleId = content.Id,
            Title1 = content.Title,
            Type = content.Type,
            Price = content.Price,
            Advance = content.Advance,
            Royalty = content.Royalty,
            YtdSales = 0,
            Notes = content.Notes,
            Pubdate = dateTime,
        });

        if (content.AuthorIds is not null)
        {
            var authors = await context.Authors.AsQueryable().Where(x => content.AuthorIds.Contains(x.AuId))
                .ToListAsync();

            await context.Titleauthors.AddRangeAsync(authors.Select(x => new Titleauthor()
            {
                AuId = x.AuId,
                TitleId = content.Id,
            }));
        }
        
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateProductAsync(string id ,ProductUpdatableContent content)
    {
        var title = await context.Titles.FindAsync(id);
        if (title == null)
        {
            return Result.Failure(new DefaultErrorMessage(404, "Title not found"));
        }
        
        title.Title1 = content.Title;
        title.Type = content.Type;
        title.Price = content.Price;
        title.Notes = content.Notes;
        
        context.Titles.Update(title);
        await context.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> DeleteProductAsync(string id)
    {
        var title = await context.Titles.FindAsync(id);
        if (title == null)
        {
            return Result.Failure(new DefaultErrorMessage(404, "Title not found"));
        }
        context.Titles.Remove(title);
        await context.SaveChangesAsync();
        return Result.Success();
    }
}