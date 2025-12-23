using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NotesApi.Contracts.Auth;
using NotesApi.Data;

namespace NotesApi.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
  private readonly UserManager<ApplicationUser> _userManager;
  private readonly IConfiguration _cfg;

  public AccountController(UserManager<ApplicationUser> userManager, IConfiguration cfg)
  {
    _userManager = userManager;
    _cfg = cfg;
  }

  [HttpPost("/register")]
  public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
  {
    var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
    var result = await _userManager.CreateAsync(user, dto.Password);

    if (!result.Succeeded)
      return BadRequest(result.Errors);

    return Ok();
  }

  [HttpPost("/login")]
  public async Task<ActionResult<string>> Login([FromBody] LoginRequest dto)
  {
    var user = await _userManager.FindByEmailAsync(dto.Email);
    if (user is null) return Unauthorized();

    var ok = await _userManager.CheckPasswordAsync(user, dto.Password);
    if (!ok) return Unauthorized();

    var token = CreateJwt(user);
    return Ok(token); // czysty string JWT — OK pod testy
  }

  private string CreateJwt(ApplicationUser user)
  {
    var key = _cfg["Jwt:Key"]!;
    var issuer = _cfg["Jwt:Issuer"]!;
    var audience = _cfg["Jwt:Audience"]!;

    var claims = new List<Claim>
    {
      new Claim(ClaimTypes.NameIdentifier, user.Id),
      new Claim(ClaimTypes.Email, user.Email ?? ""),
    };

    var creds = new SigningCredentials(
      new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
      SecurityAlgorithms.HmacSha256);

    var jwt = new JwtSecurityToken(
      issuer: issuer,
      audience: audience,
      claims: claims,
      expires: DateTime.UtcNow.AddHours(2),
      signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(jwt);
  }
}
