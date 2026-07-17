using CoreCommerce.Application.Orders.Commands.Checkout;
using FluentValidation;

namespace CoreCommerce.Application.Features.Checkout;
public class CheckoutCommandValidator : AbstractValidator<CheckoutCommand>
{
    public CheckoutCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.CartId).NotEmpty();
        RuleFor(x => x.ShippingAddress).NotEmpty().MaximumLength(500);
        RuleFor(x => x.PaymentToken).NotEmpty();
    }
}