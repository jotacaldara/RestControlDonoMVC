using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RestControlMVC.DTOs;
using RestControlMVC.Services;


namespace RestControlMVC.Controllers.Admin
{
        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        public class SubscriptionsAdminController : Controller
        {
            private readonly ApiService _apiService;

            public SubscriptionsAdminController(ApiService apiService)
            {
                _apiService = apiService;
            }

            // GET: /Admin/SubscriptionsAdmin
            public async Task<IActionResult> Index()
            {
                try
                {
                    var subs = await _apiService.GetAsync<List<AdminSubscriptionListDTO>>("subscriptions/all");
                    return View("~/Views/Admin/Subscription/Index.cshtml",
                        subs ?? new List<AdminSubscriptionListDTO>());
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erro: {ex.Message}";
                    return View("~/Views/Admin/Subscription/Index.cshtml",
                        new List<AdminSubscriptionListDTO>());
                }
            }

            // POST: /Admin/SubscriptionsAdmin/Renew
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Renew(int id)
            {
                var success = await _apiService.PostAsync($"subscriptions/renew/{id}", new { });
                TempData[success ? "Success" : "Error"] = success
                    ? "Subscrição renovada por 30 dias!"
                    : "Erro ao renovar subscrição.";
                return RedirectToAction(nameof(Index));
            }

            // POST: /Admin/SubscriptionsAdmin/Notify
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Notify(int id)
            {
                var success = await _apiService.PostAsync($"subscriptions/notify/{id}", new { });
                TempData[success ? "Success" : "Error"] = success
                    ? "Email enviado com sucesso!"
                    : "Erro ao enviar email.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
