namespace Catalog.Application.Auth;

public record LoginRequest(string Email, string Password);

public record LoginResponse(string Token, string UserId, string Email, string Role, DateTime Expiration);