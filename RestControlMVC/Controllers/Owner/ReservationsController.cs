using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.Services;
using RestControlMVC.DTOs.Owner;

namespace RestControlMVC.Owner.Controllers
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class ReservationsController : Controller
    {
        private readonly ApiService _apiService;

        public ReservationsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Owner/Reservations
        public async Task<IActionResult> Index(string status = "")
        {
            var endpoint = string.IsNullOrEmpty(status)
                ? "owner/reservations"
                : $"owner/reservations?status={status}";

            var reservations = await _apiService.GetAsync<List<ReservationDTO>>(endpoint);
            ViewBag.StatusFilter = status;

            if (reservations == null)
            {
                return View("~/Views/Owner/Reservations/Index.cshtml", new OwnerDashboardDTO());
            }

            return View("~/Views/Owner/Reservations/Index.cshtml", reservations);

        }

        // POST: Owner/Reservations/ConfirmReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmReservation(int id)
        {
            var dto = new { Status = "Confirmada" };
            var success = await _apiService.PutAsync($"owner/reservations/{id}/status", dto);

            if (success)
                TempData["Success"] = "Reserva confirmada!";
            else
                TempData["Error"] = "Erro ao confirmar reserva.";

            return RedirectToAction(nameof(Index));
        }

        // POST: Owner/Reservations/CancelReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var dto = new { Status = "Cancelada" };
            var success = await _apiService.PutAsync($"owner/reservations/{id}/status", dto);

            if (success)
                TempData["Success"] = "Reserva cancelada.";
            else
                TempData["Error"] = "Erro ao cancelar reserva.";

            return RedirectToAction(nameof(Index));
        }

        // POST: Owner/Reservations/AddAmount/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAmount(int id, decimal amount, string paymentMethod)
        {
            var dto = new { Amount = amount, PaymentMethod = paymentMethod };
            var success = await _apiService.PutAsync($"owner/reservations/{id}/amount", dto);

            if (success)
                TempData["Success"] = "Valor adicionado com sucesso!";
            else
                TempData["Error"] = "Erro ao adicionar valor.";

            return RedirectToAction(nameof(Index));
        }
    }
}