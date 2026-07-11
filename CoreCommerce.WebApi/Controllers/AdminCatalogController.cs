using CoreCommerce.Application.Features.Catalog.Commands.CreateCategory;
using CoreCommerce.Application.Features.Catalog.Commands.CreateProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreCommerce.WebApi.Controllers;

[ApiController]
[Route("api/admin/catalog")]
[Authorize(Roles = "Admin")]
public class AdminCatalogController : ControllerBase
{
    private readonly ISender _mediator;

    public AdminCatalogController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var categoryId = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateCategory), new { id = categoryId }, categoryId);
    }

    [HttpPost("products")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(CreateProduct), new { id = productId }, productId);
    }
}