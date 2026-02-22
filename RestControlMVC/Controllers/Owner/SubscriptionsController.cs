using RestControlMVC.DTOs;
using RestControlMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestControlMVC.Controllers.Owner
{
        [Area("Owner")]
        [Authorize(Roles = "Owner")]
        public class SubscriptionController : Controller
        {
            private readonly ApiService _apiService;

            public SubscriptionController(ApiService apiService)
            {
                _apiService = apiService;
            }

            // GET: Owner/Subscription
            public async Task<IActionResult> Index()
            {
                try
                {
                    var subscription = await _apiService.GetAsync<SubscriptionDTO>("owner/subscription");

                    if (subscription == null)
                    {
                        TempData["Error"] = "Nenhuma subscrição ativa encontrada. Entre em contato com o administrador.";
                        return View("~/Views/Owner/Subscription/Index.cshtml", new SubscriptionDTO());
                    }

                    // Verificar se está expirando (5 dias ou menos)
                    if (subscription.IsExpiring)
                    {
                        TempData["Warning"] = $"Atenção! Sua subscrição expira em {subscription.DaysRemaining} dias. Renove agora para evitar interrupções no serviço.";
                    }

                    return View("~/Views/Owner/Subscription/Index.cshtml", subscription);
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Erro ao carregar subscrição: {ex.Message}";
                    return View(new SubscriptionDTO());
                }
            }

            // POST: Owner/Subscription/RequestRenewal
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> RequestRenewal()
            {
                //CRIAR SERVIÇO DE EMAIL PARA ENVIAR NOTIFICAÇÃO AO ADMINISTRADOR
                TempData["Info"] = "Solicitação de renovação enviada! O administrador entrará em contato em breve. " +
                                   "Você também pode contactar o suporte: suporte@restflow.com";

                return RedirectToAction(nameof(Index));
            }
        }
    }
