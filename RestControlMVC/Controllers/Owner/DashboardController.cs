
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs;
using RestControlMVC.DTOs.Owner;
using RestControlMVC.Services;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RestControlMVC.Owner.Controllers
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
                    TempData["Error"] = "Erro ao carregar dados do dashboard.";
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

        // GET: /Owner/Dashboard/RestaurantDetail
        public async Task<IActionResult> RestaurantDetail()
        {
            try
            {
                var restaurant = await _apiService.GetAsync<RestaurantDetailDTO>("owner/restaurant");

                if (restaurant == null)
                {
                    TempData["Error"] = "Restaurante não encontrado.";
                    return RedirectToAction("Index");
                }

                return View("~/Views/Owner/Dashboard/RestaurantDetail.cshtml", restaurant);
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

                // Mapeia para o DTO de edição
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
                    return RedirectToAction("RestaurantDetail");
                }
                else
                {
                    TempData["Error"] = "Erro ao atualizar o restaurante.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Reviews()
        {
            try
            {
                var reviews = await _apiService.GetAsync<List<ReviewDTO>>("owner/reviews");
                return View("~/Views/Owner/Dashboard/Reviews.cshtml", reviews);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return View(new List<ReviewDTO>());
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyReview(int reviewId, string reply)
        {
            if (string.IsNullOrWhiteSpace(reply))
            {
                TempData["Error"] = "A resposta não pode estar vazia.";
                return RedirectToAction("Reviews");
            }

            try
            {
                var dto = new { Reply = reply };
                var success = await _apiService.PostAsync($"owner/reviews/{reviewId}/reply", dto);

                if (success)
                    TempData["Success"] = "Resposta enviada com sucesso!";
                else
                    TempData["Error"] = "Erro ao enviar resposta.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
            }

            return RedirectToAction("Reviews");
        }
    }
}