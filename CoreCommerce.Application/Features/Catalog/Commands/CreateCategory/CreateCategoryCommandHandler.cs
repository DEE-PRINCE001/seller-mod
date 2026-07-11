using CoreCommerce.Application.Common.Interfaces;
using CoreCommerce.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreCommerce.Application.Features.Catalog.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var slugExists = await _context.Categories
            .AnyAsync(c => c.Slug == request.Slug, cancellationToken);

        if (slugExists)
        {
            throw new Exception("A category with this slug already exists.");
        }

        var category = new Category
        {
            Name = request.Name,
            Slug = request.Slug.ToLower().Trim()
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}