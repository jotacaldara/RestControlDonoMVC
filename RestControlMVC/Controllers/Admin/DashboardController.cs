using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.Services;
using RestControlMVC.DTOs;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class DashboardController : Controller
{
    private readonly ApiService _apiService;

    public DashboardController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        //(Total de Restaurantes, Receita, Reservas, Pendentes)
        var stats = await _apiService.GetAsync<DashboardKpiDTO>("admin/admindashboard/stats");

        return View("~/Views/Admin/Dashboard/Index.cshtml", stats ?? new DashboardKpiDTO());
    }

    [HttpGet]
    public async Task<IActionResult> GetChartData()
    {
        //EndPoint
        var data = await _apiService.GetAsync<List<RevenueDataDTO>>("admin/admindashboard/revenue-data");
        return Json(data ?? new List<RevenueDataDTO>());
    }
}