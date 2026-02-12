using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestControlMVC.DTOs;
using RestControlMVC.DTOs.Owner;
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

            // GET: Owner/Menu
            public async Task<IActionResult> Index()
            {
                var categories = await _apiService.GetAsync<List<CategoryDTO>>("owner/menu");

            if (categories == null)
            {
                return View("~/Views/Owner/Menu/Index.cshtml", new OwnerDashboardDTO());
            }

            return View("~/Views/Owner/Menu/Index.cshtml", categories);
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

                var dto = new { Name = name };
                var success = await _apiService.PostAsync("api/owner/categories", dto);

                if (success)
                    TempData["Success"] = "Categoria criada com sucesso!";
                else
                    TempData["Error"] = "Erro ao criar categoria.";

                return RedirectToAction(nameof(Index));
            }

            // POST: Owner/Menu/DeleteCategory/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteCategory(int id)
            {
                var success = await _apiService.DeleteAsync($"api/owner/categories/{id}");

                if (success)
                    TempData["Success"] = "Categoria removida!";
                else
                    TempData["Error"] = "Erro ao remover categoria.";

                return RedirectToAction(nameof(Index));
            }

            // POST: Owner/Menu/CreateProduct
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> CreateProduct(ProductDto dto)
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = "Preencha todos os campos obrigatórios.";
                    return RedirectToAction(nameof(Index));
                }

                var success = await _apiService.PostAsync("api/owner/products", dto);

                if (success)
                    TempData["Success"] = "Produto criado com sucesso!";
                else
                    TempData["Error"] = "Erro ao criar produto.";

                return RedirectToAction(nameof(Index));
            }

            // POST: Owner/Menu/UpdateProduct/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> UpdateProduct(int id, ProductDto dto)
            {
                var success = await _apiService.PutAsync($"api/owner/products/{id}", dto);

                if (success)
                    TempData["Success"] = "Produto atualizado!";
                else
                    TempData["Error"] = "Erro ao atualizar produto.";

                return RedirectToAction(nameof(Index));
            }

            // POST: Owner/Menu/DeleteProduct/5
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteProduct(int id)
            {
                var success = await _apiService.DeleteAsync($"api/owner/products/{id}");

                if (success)
                    TempData["Success"] = "Produto removido!";
                else
                    TempData["Error"] = "Erro ao remover produto.";

                return RedirectToAction(nameof(Index));
            }
        }
    }

