// Local: Controllers/Admin/DashboardController.cs
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
        // Bate no endpoint da sua API
        var stats = await _apiService.GetAsync<DashboardKpiDTO>("admin/admindashboard/stats");
        return View("~/Views/Admin/Dashboard/Index.cshtml", stats);
    }


}