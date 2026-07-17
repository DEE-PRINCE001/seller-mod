namespace CoreCommerce.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<bool> ProcessPaymentAsync(Guid orderId, decimal amount, string paymentToken, CancellationToken cancellationToken);
}