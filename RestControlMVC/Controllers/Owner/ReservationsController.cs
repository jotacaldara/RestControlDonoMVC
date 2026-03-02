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
            // Always fetch ALL reservations so counters are always accurate
            var allReservations = await _apiService.GetAsync<List<ReservationDTO>>("owner/reservations")
                                  ?? new List<ReservationDTO>();

            // Pass real totals to the view via ViewBag — independent of the active filter
            ViewBag.StatusFilter = status;
            ViewBag.TotalCount = allReservations.Count;
            ViewBag.PendentCount = allReservations.Count(r => r.Status == "Pending");
            ViewBag.ConfirmedCount = allReservations.Count(r => r.Status == "Confirmed");
            ViewBag.CanceledCount = allReservations.Count(r => r.Status == "Canceled");

            // Filter the model that is rendered in the table
            var filtered = string.IsNullOrEmpty(status)
                ? allReservations
                : allReservations.Where(r => r.Status == status).ToList();

            return View("~/Views/Owner/Reservations/Index.cshtml", filtered);
        }

        // POST: Owner/Reservations/ConfirmReservation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmReservation(int id)
        {
            if (id <= 0)
            {
                TempData["Error"] = "ID da reserva inválido.";
                return RedirectToAction(nameof(Index));
            }

            var dto = new { Status = "Confirmed" };
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
            // FIX: was "Cancelada" — standard is "Canceled"
            var dto = new { Status = "Canceled" };
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