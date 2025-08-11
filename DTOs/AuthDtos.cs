
namespace TaskManager.Api.DTOs;

public record RegisterRequest(string UserName, string Password);
public record LoginRequest(string UserName, string Password);
public record AuthResponse(string Token);
