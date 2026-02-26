using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using RestControlMVC.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configuração da URL da API
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7149/api/";
if (!apiBaseUrl.EndsWith("/")) apiBaseUrl += "/";

var supportedCultures = new[] { new CultureInfo("en-US"), new CultureInfo("pt-PT") };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US"); 
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// 1. Serviços de Cookies (Obrigatório para o Banner)
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true; 
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

// 2. Autenticação
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.Cookie.Name = "RestControlAuth";
    });

// 3. Injeção de Dependência (Sem duplicatas)
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();

// HttpClient Centralizado
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCookiePolicy(); // Antes de Authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "MyAreas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();