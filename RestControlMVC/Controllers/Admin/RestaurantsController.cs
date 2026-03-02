using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs;
using RestControlMVC.Services;

namespace RestControlMVC.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RestaurantsController : Controller
    {
        private readonly ApiService _apiService;

        public RestaurantsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string city = null)
        {
            // Chama api/restaurants com filtro opcional de cidade
            var endpoint = string.IsNullOrEmpty(city) ? "restaurants" : $"restaurants?city={city}";
            var restaurants = await _apiService.GetAsync<IEnumerable<RestaurantListDTO>>(endpoint);

            return View("~/Views/Admin/Restaurants/Index.cshtml", restaurants ?? new List<RestaurantListDTO>());
        }

        public async Task<IActionResult> Details(int id)
        {
            // Chama o endpoint GET api/restaurants/{id} da sua API
            var restaurant = await _apiService.GetAsync<RestaurantDetailDTO>($"restaurants/{id}");

            if (restaurant == null) return NotFound();

            return View("~/Views/Admin/Restaurants/Details.cshtml", restaurant);
        }

       

        // Método para processar a aprovação
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _apiService.PostAsync<object>($"admin/admindashboard/approve/{id}", null);
            return RedirectToAction("Pending");
        }

        // POST: Admin/Restaurants/Deactivate/5
        [HttpPost]
        public async Task<IActionResult> Deactivate(int id)
        {
            var result = await _apiService.PostAsync<object>($"restaurants/deactivate/{id}", null);

            if (result != null)
                TempData["Success"] = "Restaurante desativado com sucesso.";
            else
                TempData["Error"] = "Erro ao desativar restaurante.";

            return RedirectToAction("Details", new { id = id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var restaurant = await _apiService.GetAsync<RestaurantDetailDTO>($"restaurants/{id}");

            if (restaurant == null) return NotFound();

            return View("~/Views/Admin/Restaurants/Edit.cshtml", restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RestaurantDetailDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Admin/Restaurants/Edit.cshtml", model);
            }

            var result = await _apiService.PutAsync<bool>($"restaurants/edit/{model.Id}", model);

            if (result)
            {
                TempData["Success"] = "Restaurante atualizado com sucesso!";
                return RedirectToAction("Details", new { id = model.Id });
            }

            TempData["Error"] = "Erro ao atualizar restaurante na API.";
            return View("~/Views/Admin/Restaurants/Edit.cshtml", model);
        }
    }
}
