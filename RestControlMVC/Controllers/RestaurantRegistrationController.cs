using Microsoft.AspNetCore.Mvc;
using RestControlMVC.Services;
using RestControlMVC.DTOs;

namespace RestControlMVC.Controllers
{
    public class RestaurantRegistrationController : Controller
    {
        private readonly ApiService _apiService;

        public RestaurantRegistrationController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /RestaurantRegistration/Register
        public IActionResult Register()
        {
            return View("~/Views/Home/Register.cshtml");
        }

        // POST: /RestaurantRegistration/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RestaurantRegistrationDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Home/Register.cshtml", model);
            }

            try
            {
                var success = await _apiService.PostAsync("registration/restaurant", model);

                if (success)
                {
                    return View("~/Views/Home/RegisterSuccess.cshtml");
                }
                else
                {
                    TempData["Error"] = "Erro ao submeter pedido. Tente novamente.";
                    return View("~/Views/Home/Register.cshtml", model);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return View("~/Views/Home/Register.cshtml", model);
            }
        }
        public IActionResult Success()
        {
            return View("~/Views/Home/RegisterSuccess.cshtml");
        }
    }
}
