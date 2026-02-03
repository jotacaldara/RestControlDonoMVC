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

        public async Task<IActionResult> Pending()
        {
            var pending = await _apiService.GetAsync<IEnumerable<RestaurantListDTO>>("admin/admindashboard/pending");
            return View("~/Views/Admin/Restaurants/Pending.cshtml", pending ?? new List<RestaurantListDTO>());
        }

        // Método para processar a aprovação
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _apiService.PostAsync<object>($"admin/admindashboard/approve/{id}", null);
            return RedirectToAction("Pending");
        }
    }
}
