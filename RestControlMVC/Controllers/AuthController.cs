using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs;
using RestControlMVC.Services;
using RestControlMVC.ViewModels.Auth;
using System.Security.Claims;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet] // Adicione este atributo explicitamente
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var loginDto = new LoginDto { Email = model.Email, Password = model.Password };

        // O AuthService agora vai encontrar o LoginResponseDTO
        var response = await _authService.LoginAsync(loginDto);

        if (response != null)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, response.Name),
            new Claim(ClaimTypes.Role, response.Role),
            new Claim("UserId", response.UserId.ToString())
        };

            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");
            await HttpContext.SignInAsync("CookieAuth", new ClaimsPrincipal(claimsIdentity));

            // Redirecionamento baseado na Role exata da API
            return response.Role == "Admin"
                ? RedirectToAction("Index", "Dashboard", new { area = "Admin" })
                : RedirectToAction("Index", "Home");
        }

        // Se cair aqui, a API retornou null ou erro
        ModelState.AddModelError("", "E-mail ou senha inválidos.");
        return View(model);
    }
}