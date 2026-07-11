using FluentValidation;

namespace CoreCommerce.Application.Features.Catalog.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .Matches(@"^[a-z0-9-]+$").WithMessage("Slug must be lowercase alphanumeric characters or hyphens only.")
            .MaximumLength(100).WithMessage("Slug must not exceed 100 characters.");
    }
}