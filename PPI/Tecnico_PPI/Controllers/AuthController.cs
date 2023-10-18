using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PPI.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Método para obtener un token
    /// </summary>
    /// <param name="login">Revisar el esquema de la clase</param>
    /// <returns></returns>
    [AllowAnonymous] // Permite el acceso público a este endpoint
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDTO login)
    {
        if (IsValidUser(login.Username, login.Password))
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MiClaveSecreta12345"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "PPI-TEST",
                audience: "PPI-TEST",
                claims: new[] { new Claim(ClaimTypes.Name, login.Username) },
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        return Unauthorized();
    }

    private bool IsValidUser(string username, string password)
    {
        var usernameSetting = _configuration["AuthenticationSettings:Username"];
        var passwordAppSetting = _configuration["AuthenticationSettings:Password"];

        if (usernameSetting == username && passwordAppSetting == password)
            return true;
        else
            return false;
    }
}
