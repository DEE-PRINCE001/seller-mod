using MediatR;
using Microsoft.EntityFrameworkCore;
using CoreCommerce.Application.Common.Interfaces;

namespace CoreCommerce.Application.Features.Auth.Commands.ResetPassword;


public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => 
            u.PasswordResetToken == request.Token && 
            u.PasswordResetTokenExpiryUtc > DateTime.UtcNow, cancellationToken);

        if (user == null)
        {
            throw new Exception("Invalid or expired reset token.");
        }

        user.PasswordHash = _passwordHasher.HashPassword(request.NewPassword);
        user.PasswordResetToken = null; 
        user.PasswordResetTokenExpiryUtc = null;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}