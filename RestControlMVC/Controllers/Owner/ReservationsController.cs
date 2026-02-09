using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs.Owner;
using RestControlMVC.Services;

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

        // GET: /Owner/Reservations/Index
        public async Task<IActionResult> Index(string? status = null)
        {
            try
            {
                var endpoint = "api/owner/reservations";

                if (!string.IsNullOrWhiteSpace(status))
                {
                    endpoint += $"?status={status}";
                }

                var reservations = await _apiService.GetAsync<List<ReservationDTO>>(endpoint);

                if (reservations == null)
                {
                    reservations = new List<ReservationDTO>();
                    TempData["Info"] = "Nenhuma reserva encontrada.";
                }

                ViewBag.CurrentStatus = status;
                return View(reservations);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao carregar reservas: {ex.Message}";
                return View(new List<ReservationDTO>());
            }
        }
    }
}
