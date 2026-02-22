using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.Services;
using RestControlMVC.DTOs;

namespace RestControlMVC.Controllers.Owner
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class PaymentsController : Controller
    {
        private readonly ApiService _apiService;

        public PaymentsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Owner/Payments
        public async Task<IActionResult> Index()
        {
            try
            {
                var payments = await _apiService.GetAsync<PaymentHistoryDTO>("owner/payments");

                if (payments == null)
                {
                    TempData["Error"] = "Erro ao carregar histórico de pagamentos.";
                    return View(new PaymentHistoryDTO());
                }

                return View(payments);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return View(new PaymentHistoryDTO());
            }
        }
    }
}
