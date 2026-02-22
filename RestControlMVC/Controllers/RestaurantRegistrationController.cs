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
                return View(model);
            }

            try
            {
                var success = await _apiService.PostAsync("registration/restaurant", model);

                if (success)
                {
                    return RedirectToAction("RegisterSuccess");
                }
                else
                {
                    TempData["Error"] = "Erro ao submeter pedido. Tente novamente.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return View(model);
            }
        }

        // GET: /RestaurantRegistration/Success
        public IActionResult Success()
        {
            return View();
        }
    }
}
