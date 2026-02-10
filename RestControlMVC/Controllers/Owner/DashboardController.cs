using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs;
using RestControlMVC.DTOs.Owner;
using RestControlMVC.Services;

namespace RestControlMVC.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class DashboardController : Controller
    {
        private readonly ApiService _apiService;

        public DashboardController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Owner/Dashboard/Index
        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboardData = await _apiService.GetAsync<OwnerDashboardDTO>("owner/dashboard");

                if (dashboardData == null)
                {
                    // Se falhar, retorna a view com caminho absoluto para evitar erro 404
                    return View("~/Views/Owner/Dashboard/Index.cshtml", new OwnerDashboardDTO());
                }
                return View("~/Views/Owner/Dashboard/Index.cshtml", dashboardData);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao carregar dashboard: {ex.Message}";
                return View(new OwnerDashboardDTO());
            }


        }

        // GET: /Owner/Dashboard/RestaurantDetails
        public async Task<IActionResult> RestaurantDetails()
        {
            try
            {
                var restaurant = await _apiService.GetAsync<RestaurantDetailDTO>("owner/restaurant");

                if (restaurant == null)
                {
                    TempData["Error"] = "Restaurante não encontrado.";
                    return RedirectToAction("Index");
                }

                return View("~/Views/Owner/Dashboard/RestaurantDetails.cshtml", restaurant);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // GET: /Owner/Dashboard/Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            try
            {
                var restaurant = await _apiService.GetAsync<RestaurantDetailDTO>("owner/restaurant");

                if (restaurant == null)
                {
                    TempData["Error"] = "Restaurante não encontrado.";
                    return RedirectToAction("Index");
                }

                // Mapeia para o DTO de edição (apenas campos permitidos)
                var editDto = new RestaurantEditDTO
                {
                    Description = restaurant.Description,
                    Phone = restaurant.Phone
                };

                return View("~/Views/Owner/Dashboard/Edit.cshtml", editDto);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        // POST: /Owner/Dashboard/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RestaurantEditDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var updateDto = new
                {
                    Description = model.Description,
                    Phone = model.Phone
                };

                var success = await _apiService.PutAsync("owner/restaurant", updateDto);

                if (success)
                {
                    TempData["Success"] = "Restaurante atualizado com sucesso!";
                    return RedirectToAction("RestaurantDetails");
                }
                else
                {
                    TempData["Error"] = "Erro ao atualizar o restaurante.";
                    return View("~/Views/Owner/Dashboard/Edit.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return View(model);
            }
        }
    }
}