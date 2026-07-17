using CoreCommerce.Application.Common.Interfaces;
using CoreCommerce.Domain.Entities;
using CoreCommerce.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreCommerce.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResultDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;


    public RegisterUserCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterUserResultDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Check if email already exists
        var emailExists = await _context.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (emailExists)
        {
            throw new Exception("Email is already registered."); 
        }

        
        var hashedPassword = _passwordHasher.HashPassword(request.Password);

        // 3. Map inputs to Domain Entity
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = hashedPassword,
            Role = UserRole.Admin
        };

        // 4. Persist data to PostgreSQL
        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Return safe DTO result
        return new RegisterUserResultDto(user.Id, user.FirstName, user.LastName, user.Email);
    }
}