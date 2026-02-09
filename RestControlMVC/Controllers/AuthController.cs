// ==============================================================================
// ARQUIVO: RestControlMVC/Controllers/AuthController.cs
// SUBSTITUA SEU AuthController ATUAL POR ESTE
// ==============================================================================

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs;
using RestControlMVC.Services;
using System.Security.Claims;

namespace RestControlMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //salvar claims e fazer login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var loginDto = new LoginDto
                {
                    Email = model.Email,
                    Password = model.Password
                };

                //  Chamar a API através do AuthService
                var response = await _authService.LoginAsync(loginDto);

                //  Se o login falhou (API retornou null ou 401)
                if (response == null)
                {
                    ModelState.AddModelError("", "E-mail ou senha inválidos.");
                    return View(model);
                }

            // Claims são informações sobre o usuário que ficam salvas no Cookie.
            //Authorization: Bearer { valor_do_token}
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, response.UserId.ToString()),
                    new Claim(ClaimTypes.Name, response.Name),
                    new Claim(ClaimTypes.Email, response.Email),
                    new Claim(ClaimTypes.Role, response.Role),
                    new Claim("UserId", response.UserId.ToString()),
                    new Claim("Token", response.Token) 
                };

                // Se for Owner e tiver RestaurantId, adiciona também
                if (response.RestaurantId.HasValue)
                {
                    claims.Add(new Claim("RestaurantId", response.RestaurantId.Value.ToString()));
                }

                // 5. Criar identidade e assinar o Cookie
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe, // Lembrar-me
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8) // Cookie expira em 8h
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // 6. Redirecionar baseado na Role
                return response.Role switch
                {
                    "Admin" => RedirectToAction("Index", "Dashboard", new { area = "Admin" }),
                    "Owner" => RedirectToAction("Index", "Dashboard", new { area = "Owner" }),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            catch (Exception ex)
            {
                // Log do erro (em produção, use ILogger)
                System.Diagnostics.Debug.WriteLine($"Erro no Login: {ex.Message}");
                ModelState.AddModelError("", "Ocorreu um erro ao processar o login. Tente novamente.");
                return View(model);
            }
        }

        // Remove o cookie de autenticação tambem
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
