using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs;
using RestControlMVC.Services;
using System.Numerics;

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
                    var pending = await _apiService.GetAsync<List<PendingRestaurantsDTO>>("admin/admindashboard/pending-restaurants");
                return View("~/Views/Admin/PendingRestaurants/Index.cshtml", pending ?? new List<PendingRestaurantsDTO>());
            }

                catch (Exception ex)
                {

                    TempData["Error"] = $"Erro: {ex.Message}";
                    return View("~/Views/Admin/PendingRestaurants/Index.cshtml", new List<PendingRestaurantsDTO>());
                }
            }

            // POST: /Admin/PendingRestaurants/Approve/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Approve(int id, int planId)
            {
                try
                {
                if (planId <= 0)
                {
                    TempData["Error"] = "Por favor, selecione um plano.";
                    return RedirectToAction(nameof(Index));
                }

                var dto = new { PlanId = planId };

                var success = await _apiService.PostAsync($"admin/admindashboard/pending-restaurants/{id}/approve", dto);

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
                    var success = await _apiService.PostAsync($"admin/admindashboard/pending-restaurants/{id}/reject", new { reason });

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
