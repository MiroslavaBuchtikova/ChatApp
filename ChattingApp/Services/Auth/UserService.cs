using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChattingApp.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChattingApp.Services.Auth;
public class UserService : IUserService
{
    private readonly JwtSettings _configuration;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<UserService> _logger;

    public UserService(IOptions<JwtSettings> configuration, SignInManager<IdentityUser> signInManager, ILogger<UserService> logger, RoleManager<IdentityRole> roleManager)
    {
        _signInManager = signInManager;
        _logger = logger;
        _roleManager = roleManager;
        _configuration = configuration.Value;
    }

    public async Task<AuthResult> LoginAsync(LoginModel login)
    {
        var user = await _signInManager.UserManager.FindByEmailAsync(login.Email) ??
                   await _signInManager.UserManager.FindByNameAsync(login.Email);

        if (user is null) return BadAuthResult;

        var loginResult = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);

        if (!loginResult.Succeeded) return BadAuthResult;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.JwtSecurityKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = await _signInManager.UserManager.GetClaimsAsync(user);
        claims.Add(new Claim("email", user.Email));
        var token = new JwtSecurityToken(
            _configuration.JwtIssuer,
            _configuration.JwtAudience,
            claims,
            expires: DateTime.UtcNow.AddDays(_configuration.JwtExpiryInDays),
            signingCredentials: credentials);

        return new AuthResult
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Message = "Hello"
        };
    }

    public async Task<AuthResult> RegisterUser(RegisterModel registration)
    {
        var user = await _signInManager.UserManager.FindByEmailAsync(registration.Email) ??
                   await _signInManager.UserManager.FindByNameAsync(registration.Email);

        if (user is not null)
            return new AuthResult
            {
                Success = false,
                Message = "User already exists"
            };
        var creationResult = await _signInManager.UserManager.CreateAsync(new IdentityUser
        {
            Email = registration.Email,
            UserName = registration.UserName
        }, registration.Password);

        if (!creationResult.Succeeded)
            return new AuthResult
            {
                Success = false,
                Message = creationResult.Errors.FirstOrDefault()?.Description ?? "Error creating account"
            };

        return new AuthResult
        {
            Success = true,
            Message = "Successful registration"
        };
    }

    private static AuthResult BadAuthResult => new()
    {
        Success = false,
        Message = "Username or password invalid"
    };

    private async Task AddRoleIfNotPresent(string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
            await _roleManager.CreateAsync(new IdentityRole(role));
    }
}
