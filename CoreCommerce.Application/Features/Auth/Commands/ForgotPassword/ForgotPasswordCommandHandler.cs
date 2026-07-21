using MediatR;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Application.Common.Interfaces;

namespace CoreCommerce.Application.Features.Auth.Commands.ForgotPassword;


public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, string>
{
    private readonly IApplicationDbContext _context;

    public ForgotPasswordCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        if (user == null)
        {
            return "If the email matches an account, a reset token has been generated.";
        }

        var resetToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        user.PasswordResetToken = resetToken;
        user.PasswordResetTokenExpiryUtc = DateTime.UtcNow.AddMinutes(15); 

        await _context.SaveChangesAsync(cancellationToken);
        return resetToken;
    }
}