using Common.ResultPattern;
using Microsoft.AspNetCore.Mvc;
using NorthwindPubsApi.Controllers.Product.Models;
using Repositories.Abstract;
using Repositories.Models.Product;

namespace NorthwindPubsApi.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductRepository productRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ProductQueryResponse>> GetAsync([FromQuery] ProductQueryRequest query)
        {
            var queryResult = await productRepository.GetProductsAsync(new ProductQuery()
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TitleStartWith = query.Title
            });
            
            return Ok(new ProductQueryResponse()
            {
                Page = query.PageNumber,
                Count = query.PageSize,
                Products = queryResult.Select(x => new ProductQueryResponse.ProductView
                {
                    Id = x.Id,
                    Title = x.Title,
                    Type = x.Type,
                    Price = x.Price,
                    PublishDate = x.PublishDate,
                    Publisher = x.Publisher?.Name,
                }),
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessageBase))]
        public async Task<ActionResult<ProductDetailResponse>> GetByIdAsync(string id)
        {
            var result = await productRepository.GetProductByIdAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            var productDetail = result.Value!;

            return Ok(new ProductDetailResponse
            {
                Id = productDetail.Id,
                Title = productDetail.Title,
                Type = productDetail.Type,
                Price = productDetail.Price,
                PublishDate = productDetail.PublishDate,
                Publisher = productDetail.Publisher?.Name,
                Notes = productDetail.Notes,
                Author = productDetail.Author,
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessageBase))]
        public async Task<ActionResult<ProductId>> PostAsync([FromBody] ProductCreateRequest request)
        {
            var result = await productRepository.AddProductAsync(new ProductContent
            {
                Id = request.Id,
                Title = request.Title,
                Type = request.Type,
                Price = request.Price,
                Advance = request.Advance,
                Royalty = request.Royalty,
                Notes = request.Notes,
                PublisherId = request.PublisherId,
                AuthorId = request.AuthorId
            });

            if (!result.IsSuccess)
            {
                return StatusCode(result.Error!.Code, result.Error);
            }

            return Ok(new ProductId()
            {
                Id = request.Id
            });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessageBase))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessageBase))]
        public async Task<ActionResult<ProductId>> PutAsync(string id, [FromBody] ProductUpdateRequest request)
        {
            var result = await productRepository.UpdateProductAsync(id, new ProductUpdatableContent
            {
                Notes = request.Notes,
                Title = request.Title,
                Type = request.Type,
                Price = request.Price
            });

            if (!result.IsSuccess)
            {
                return StatusCode(result.Error!.Code, result.Error);
            }

            return Ok(new ProductId()
            {
                Id = id
            });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessageBase))]
        public async Task<ActionResult<ProductId>> DeleteAsync(string id)
        {
            var result = await productRepository.DeleteProductAsync(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }

            return Ok(new ProductId()
            {
                Id = id
            });
        }
    }
}