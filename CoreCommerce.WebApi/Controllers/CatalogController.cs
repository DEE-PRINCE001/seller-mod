using MediatR;
using Microsoft.AspNetCore.Mvc;
using CoreCommerce.Application.Features.Catalog.DTOs;
using CoreCommerce.Application.Features.Catalog.Queries;
using CoreCommerce.Application.Features.Catalog.Queries.GetProduct;

namespace CoreCommerce.WebApi.Controllers;

[ApiController]
[Route("api/catalog")]
public class CatalogController : ControllerBase
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("categories")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryDto>))]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCategoriesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("products")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedList<ProductDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}