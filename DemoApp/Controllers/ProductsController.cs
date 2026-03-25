using DemoApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // ── In-memory seed data (no DB needed for demo) ──────────
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Widget A", Description = "A great widget",   Price = 9.99m,  Stock = 100 },
        new Product { Id = 2, Name = "Widget B", Description = "An even better one", Price = 19.99m, Stock = 50  },
        new Product { Id = 3, Name = "Gadget X", Description = "Latest gadget",    Price = 49.99m, Stock = 25  },
    };

    // GET api/products
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
        => Ok(_products);

    // GET api/products/{id}
    [HttpGet("{id:int}")]
    public ActionResult<Product> GetById(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        return product is null ? NotFound(new { message = $"Product {id} not found." }) : Ok(product);
    }

    // POST api/products
    [HttpPost]
    public ActionResult<Product> Create([FromBody] Product product)
    {
        product.Id = _products.Max(p => p.Id) + 1;
        _products.Add(product);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT api/products/{id}
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Product updated)
    {
        var existing = _products.FirstOrDefault(p => p.Id == id);
        if (existing is null) return NotFound(new { message = $"Product {id} not found." });

        existing.Name        = updated.Name;
        existing.Description = updated.Description;
        existing.Price       = updated.Price;
        existing.Stock       = updated.Stock;

        return NoContent();
    }

    // DELETE api/products/{id}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);
        if (product is null) return NotFound(new { message = $"Product {id} not found." });

        _products.Remove(product);
        return NoContent();
    }
}
