using System.Net;
using System.Net.Http.Json;
using DemoApp.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DemoApp.Tests;

// ── Integration tests using the built-in WebApplicationFactory ──
public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductsApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithProducts()
    {
        var response = await _client.GetAsync("/api/products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.NotNull(products);
        Assert.NotEmpty(products);
    }

    [Fact]
    public async Task GetById_ExistingId_ReturnsProduct()
    {
        var response = await _client.GetAsync("/api/products/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var product = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public async Task GetById_NonExistentId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/products/9999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Create_ValidProduct_ReturnsCreated()
    {
        var newProduct = new Product
        {
            Name        = "Test Product",
            Description = "Created in test",
            Price       = 5.99m,
            Stock       = 10
        };

        var response = await _client.PostAsJsonAsync("/api/products", newProduct);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var created = await response.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(created);
        Assert.Equal("Test Product", created.Name);
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task HealthCheck_ReturnsOk()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

// ── Pure unit tests (no HTTP) ────────────────────────────────────
public class ProductModelTests
{
    [Fact]
    public void Product_DefaultValues_AreCorrect()
    {
        var product = new Product();

        Assert.Equal(0,             product.Id);
        Assert.Equal(string.Empty,  product.Name);
        Assert.Equal(string.Empty,  product.Description);
        Assert.Equal(0m,            product.Price);
        Assert.Equal(0,             product.Stock);
    }

    [Theory]
    [InlineData("Widget",  9.99,  100)]
    [InlineData("Gadget",  49.99, 25)]
    [InlineData("Tool",    1.50,  500)]
    public void Product_SetProperties_ReturnsCorrectValues(string name, decimal price, int stock)
    {
        var product = new Product { Name = name, Price = price, Stock = stock };

        Assert.Equal(name,  product.Name);
        Assert.Equal(price, product.Price);
        Assert.Equal(stock, product.Stock);
    }
}
