using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs;
using RestControlMVC.Services;

namespace RestControlMVC.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PlansController : Controller
    {
        private readonly ApiService _apiService;

        public PlansController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var expiringSubs = await _apiService.GetAsync<List<SubscriptionDTO>>("subscriptions/expiring");
            var plans = await _apiService.GetAsync<List<PlanDTO>>("plans");

            ViewBag.Plans = plans ?? new List<PlanDTO>();

            return View("~/Views/Admin/Plans/Index.cshtml", expiringSubs ?? new List<SubscriptionDTO>());
        }

        [HttpPost]
        public async Task<IActionResult> EditPlan(PlanDTO plan)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");

            // Envia a atualização para a APIt
            var result = await _apiService.PostAsync<bool>($"plans/{plan.PlanId}", plan);

            if (result) TempData["Success"] = "Plano atualizado com sucesso!";
            else TempData["Error"] = "Erro ao atualizar o plano.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SendReminder(int id)
        {
            // Chama o endpoint de notificação por email da API
            var result = await _apiService.PostAsync<object>($"subscriptions/notify/{id}", null);
            return Json(new { success = result != null });
        }
    }
}
