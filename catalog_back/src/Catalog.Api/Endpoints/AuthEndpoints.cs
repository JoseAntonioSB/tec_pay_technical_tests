using Catalog.Application.Auth;


namespace Catalog.Application.Common;

public static class AuthEndpoints
{
    private static readonly Dictionary<string, (string Password, string Role)> _users = new()
    {
        { "admin@catalog.com", ("Admin123!", "Admin") },
        { "user@catalog.com", ("User123!", "User") }
    };

    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/login", Login)
        .WithName("Login")
        .AllowAnonymous();
        return group;
    }

    private static IResult Login(LoginRequest request, ITokenService tokenService)
    {
        if (_users.TryGetValue(request.Email, out var user) && user.Password == request.Password)
        {
            var token = tokenService.GenerateToken(Guid.NewGuid().ToString(), request.Email, user.Role);
            return Results.Ok(new LoginResponse(token, request.Email, request.Email, user.Role, DateTime.UtcNow.AddHours(1)));
        }

        return Results.Unauthorized();
    }
}