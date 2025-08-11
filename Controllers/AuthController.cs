
using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.DTOs;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) { _auth = auth; }

    [HttpPost("register")]
    public Task<AuthResponse> Register(RegisterRequest req) => _auth.RegisterAsync(req);

    [HttpPost("login")]
    public Task<AuthResponse> Login(LoginRequest req) => _auth.LoginAsync(req);
}
