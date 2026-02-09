using RestControlMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7149/";

builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true; // Renova o cookie automaticamente
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS obrigatório
        options.Cookie.SameSite = SameSiteMode.Strict;
    });
// 1. Serviços de UI
builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization();

// 2. Política de Cookies (Consentimento/Pop-up)
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true; // Ativa a necessidade de aceitar o banner
    options.MinimumSameSitePolicy = SameSiteMode.None;
});


// 4. Injeção de Dependência
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();

// 5. HttpClient + ApiService
builder.Services.AddHttpClient<ApiService>(client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(baseUrl ?? "https://localhost:7149/api/");
});

var app = builder.Build();

// Configuração do Pipeline (A ORDEM IMPORTA)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// O CookiePolicy deve vir antes da Autenticação para o banner funcionar
app.UseCookiePolicy();

app.UseAuthentication();
app.UseAuthorization();

// Rotas
app.MapControllerRoute(
    name: "MyAreas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();