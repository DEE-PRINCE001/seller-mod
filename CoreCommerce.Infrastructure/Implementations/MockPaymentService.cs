using CoreCommerce.Application.Common.Interfaces;

namespace CoreCommerce.Infrastructure.Implementations;

public class MockPaymentService : IPaymentService
{
    public async Task<bool> ProcessPaymentAsync(Guid orderId, decimal amount, string paymentToken, CancellationToken cancellationToken)
    {
        
        await Task.Delay(500, cancellationToken);

        // Always succeed unless specified token indicates failure
        if (paymentToken == "fail_token")
        {
            return false;
        }

        return true;
    }
}