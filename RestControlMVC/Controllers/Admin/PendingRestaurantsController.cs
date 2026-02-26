using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.Services;
using RestControlMVC.DTOs;

namespace RestControlMVC.Controllers.Admin
{
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public class PendingRestaurantsController : Controller
        {
            private readonly ApiService _apiService;

            public PendingRestaurantsController(ApiService apiService)
            {
                _apiService = apiService;
            }

          
            public async Task<IActionResult> Index()
            {
                try
                {
                    var pending = await _apiService.GetAsync<List<PendingRestaurantDTO>>("admin/pending-restaurants");
                 return View("~/Views/Admin/PendingRestaurants/Index.cshtml", pending);
                 }

                catch (Exception ex)
                {
                    TempData["Error"] = $"Erro: {ex.Message}";
                    return View("~/Views/Admin/PendingRestaurants/Index.cshtml", new List<PendingRestaurantDTO>());
                }
            }

            // POST: /Admin/PendingRestaurants/Approve/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Approve(int id)
            {
                try
                {
                    var success = await _apiService.PostAsync($"admin/pending-restaurants/{id}/approve", new { });

                    if (success)
                        TempData["Success"] = "Restaurante aprovado com sucesso!";
                    else
                        TempData["Error"] = "Erro ao aprovar restaurante.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erro: {ex.Message}";
                }

                return RedirectToAction(nameof(Index));
            }

            // POST: /Admin/PendingRestaurants/Reject/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Reject(int id, string reason)
            {
                try
                {
                    var success = await _apiService.PostAsync($"admin/pending-restaurants/{id}/reject", new { reason });

                    if (success)
                        TempData["Success"] = "Pedido rejeitado.";
                    else
                        TempData["Error"] = "Erro ao rejeitar pedido.";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erro: {ex.Message}";
                }

                return RedirectToAction(nameof(Index));
            }
        }
    }
