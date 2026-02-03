using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.Services;
using RestControlMVC.DTOs;

namespace RestControlMVC.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApiService _apiService;

        public UsersController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // No MVC - Controllers/Admin/UsersController.cs
        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _apiService.GetAsync<IEnumerable<UserDTO>>("user");

                if (users == null || !users.Any())
                {
                    // Isso aparecerá na janela "Output" (Saída) do Visual Studio
                    System.Diagnostics.Debug.WriteLine("DEBUG: A lista retornou vazia da API.");
                    return View("~/Views/Admin/Users/Index.cshtml", new List<UserDTO>());
                }

                return View("~/Views/Admin/Users/Index.cshtml", users);
            }
            catch (Exception ex)
            {
                // Se houver erro de rede ou desserialização, você verá aqui
                System.Diagnostics.Debug.WriteLine($"CRITICAL ERROR: {ex.Message}");
                return View("~/Views/Admin/Users/Index.cshtml", new List<UserDTO>());
            }
        }
    }
}
