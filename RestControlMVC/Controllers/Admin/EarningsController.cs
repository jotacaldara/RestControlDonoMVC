using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs;
using RestControlMVC.Services;
using System.Reflection;

namespace RestControlMVC.Controllers.Admin
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class EarningsController : Controller
    {
            private readonly ApiService _apiService;

            public EarningsController(ApiService apiService)
            {
                _apiService = apiService;
            }

        public async Task<IActionResult> Index()
        {
            try
            {
                var earnings = await _apiService.GetAsync<AdminEarningsDTO>("subscriptions/earnings");

                if (earnings == null)
                {
                    TempData["Error"] = "Erro ao carregar ganhos da plataforma.";
                    return View("~/Views/Admin/Earnings/Index.cshtml", new AdminEarningsDTO());
                }

                return View("~/Views/Admin/Earnings/Index.cshtml", earnings);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return View("~/Views/Admin/Earnings/Index.cshtml", new AdminEarningsDTO());
            }
        }
    }
    }

