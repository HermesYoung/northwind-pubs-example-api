using Common.ResultPattern;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NorthwindPubsApi.Controllers.Product;
using NorthwindPubsApi.Controllers.Product.Models;
using NSubstitute;
using Repositories.Abstract;
using Repositories.Models.Product;

namespace NorthwindPubsApiTests.Controllers.Product;

public class ProductControllerTests
{
    private ProductController _controller;
    private IProductRepository _productRepository;

    [SetUp]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
        _productRepository = Substitute.For<IProductRepository>();
        serviceCollection.AddSingleton(_productRepository);
        serviceCollection.AddSingleton<ProductController>();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _controller = serviceProvider.GetRequiredService<ProductController>();
    }

    [Test]
    public async Task PostAsync_CreateProductSuccess_ReturnsOkWithId()
    {
        var request = new ProductCreateRequest
        {
            Id = "Test",
            Title = "Title",
            Type = "Test",
            Price = 50,
            AuthorId = "AuthorId",
            PublisherId = "PublisherId",
            Notes = "Notes",
            Advance = 1000,
            Royalty = 20
        };

        _productRepository.AddProductAsync(Arg.Any<ProductContent>()).Returns(Task.FromResult(Result.Success()));

        var result = await _controller.PostAsync(request);

        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(200));
        Assert.That(okResult.Value, Is.TypeOf<ProductId>());
        var product = okResult.Value as ProductId;
        Assert.That(product.Id, Is.EqualTo("Test"));
    }

    [Test]
    public async Task PostAsync_CreateProductError_ReturnsErrorStatusCode()
    {
        var request = new ProductCreateRequest
        {
            Id = "Test",
            Title = "Title",
            Type = "Test",
            Price = 50,
            AuthorId = "AuthorId",
            PublisherId = "PublisherId",
            Notes = "Notes",
            Advance = 1000,
            Royalty = 20
        };

        var errorMessage = new DefaultErrorMessage(400, "Bad Request");
        _productRepository.AddProductAsync(Arg.Any<ProductContent>())
            .Returns(Task.FromResult(Result.Failure(errorMessage)));

        var result = await _controller.PostAsync(request);

        Assert.That(result.Result, Is.InstanceOf<ObjectResult>());
        var badRequestObjectResult = result.Result as ObjectResult;
        Assert.That(badRequestObjectResult.StatusCode, Is.EqualTo(400));
        Assert.That(badRequestObjectResult.Value, Is.InstanceOf<ErrorMessageBase>());
        var product = badRequestObjectResult.Value as ErrorMessageBase;
        Assert.That(product.Message, Is.EqualTo(errorMessage.Message));
    }
}