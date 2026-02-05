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

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            // Busca os dados atuais do utilizador na API para preencher o formulário
            var user = await _apiService.GetAsync<UserDTO>($"user/{id}");

            if (user == null) return NotFound();

            return View("~/Views/Admin/Users/Edit.cshtml", user);
        }

        // POST: Admin/Users/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(UserDTO user)
        {
            if (!ModelState.IsValid) return View("~/Views/Admin/Users/Edit.cshtml", user);

            // Envia a atualização para a API (usando Put ou Post conforme sua API aceite)
            var success = await _apiService.PostAsync<bool>($"user/{user.id}", user);

            if (success)
            {
                TempData["Success"] = "Utilizador atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Erro ao atualizar utilizador na API.");
            return View("~/Views/Admin/Users/Edit.cshtml", user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _apiService.DeleteAsync($"user/{id}");

            if (success)
            {
                TempData["Success"] = "Utilizador removido com sucesso!";
            }
            else
            {
                TempData["Error"] = "Não foi possível remover o utilizador.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
