using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs;
using RestControlMVC.Services;

namespace RestControlMVC.Controllers.Owner
{
    [Area("Owner")]
    [Authorize(Roles = "Owner")]
    public class MenuController : Controller
    {
        private readonly ApiService _apiService;

        public MenuController(ApiService apiService)
        {
            _apiService = apiService;
        }

        private async Task<int?> GetOwnerRestaurantIdAsync()
        {
            var claim = User.FindFirst("RestaurantId")?.Value;

            if (string.IsNullOrEmpty(claim))
            {
                System.Diagnostics.Debug.WriteLine("[ERRO] Claim 'RestaurantId' não encontrada no usuário logado.");
                return null;
            }

            return int.Parse(claim);
        }

        // GET: Owner/Menu
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _apiService.GetAsync<List<CategoryDTO>>("owner/menu");

                if (categories == null)
                {
                    TempData["Error"] = "Erro ao carregar menu.";
                    return View("~/Views/Owner/Menu/Index.cshtml", new List<CategoryDTO>());
                }

                return View("~/Views/Owner/Menu/Index.cshtml", categories);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
                return View("~/Views/Owner/Dashboard/Index.cshtml", new List<CategoryDTO>());
            }
        }

        // POST: Owner/Menu/CreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                TempData["Error"] = "Nome da categoria é obrigatório.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var dto = new { Name = name };
                var success = await _apiService.PostAsync("owner/categories", dto);

                if (success)
                    TempData["Success"] = "Categoria criada com sucesso!";
                else
                    TempData["Error"] = "Erro ao criar categoria.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Owner/Menu/DeleteCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var success = await _apiService.DeleteAsync($"owner/categories/{id}");

                if (success)
                    TempData["Success"] = "Categoria removida!";
                else
                    TempData["Error"] = "Erro ao remover categoria.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductDto formDto)
        {
            try
            {
                var restaurantId = await GetOwnerRestaurantIdAsync();
                if (!restaurantId.HasValue)
                {
                    TempData["Error"] = "Sessão inválida ou restaurante não identificado. Tente fazer login novamente.";
                    return RedirectToAction(nameof(Index));
                }

                var isAvailable = Request.Form["IsAvailable"].ToString()
                    .Contains("true", StringComparison.OrdinalIgnoreCase);

                var apiDto = new
                {
                    RestaurantId = restaurantId.Value,
                    CategoryId = formDto.CategoryId,
                    Name = formDto.Name ?? "",
                    Description = formDto.Description ?? "",
                    Price = formDto.Price ?? 0,
                    IsAvailable = true  
                };

                var success = await _apiService.PostAsync("owner/products", apiDto);

                if (success)
                    TempData["Success"] = "Produto criado com sucesso!";
                else
                    TempData["Error"] = "Erro ao criar produto.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto formDto)
        {
            try
            {
                var restaurantId = await GetOwnerRestaurantIdAsync();
                if (!restaurantId.HasValue)
                {
                    TempData["Error"] = "Restaurante não encontrado.";
                    return RedirectToAction(nameof(Index));
                }

                var isAvailable = Request.Form["IsAvailable"].ToString()
                    .Contains("true", StringComparison.OrdinalIgnoreCase);

                var apiDto = new
                {
                    RestaurantId = restaurantId.Value,
                    CategoryId = formDto.CategoryId,
                    Name = formDto.Name ?? "",
                    Description = formDto.Description ?? "",
                    Price = formDto.Price ?? 0,
                    IsAvailable = true  
                };

                var success = await _apiService.PutAsync($"owner/products/{id}", apiDto);

                if (success)
                    TempData["Success"] = "Produto atualizado com sucesso!";
                else
                    TempData["Error"] = "Erro ao atualizar produto.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Owner/Menu/DeleteProduct/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var success = await _apiService.DeleteAsync($"owner/products/{id}");

                if (success)
                    TempData["Success"] = "Produto removido com sucesso!";
                else
                    TempData["Error"] = "Erro ao remover produto.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}