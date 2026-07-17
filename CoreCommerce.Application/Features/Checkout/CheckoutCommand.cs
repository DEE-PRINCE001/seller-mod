using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Domain.Entities;
using CoreCommerce.Domain.Enums;
using CoreCommerce.Application.Common.Interfaces;

namespace CoreCommerce.Application.Orders.Commands.Checkout;

public record CheckoutCommand : IRequest<Guid>
{
    public Guid UserId { get; init; }
    public string CartId { get; init; } = string.Empty;
    public string ShippingAddress { get; init; } = string.Empty;
    public string PaymentToken { get; init; } = string.Empty;
}



