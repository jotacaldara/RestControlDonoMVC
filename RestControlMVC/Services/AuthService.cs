using RestControlMVC.DTOs;
using System.Text.Json;

namespace RestControlMVC.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDTO?> LoginAsync(LoginDto loginDto);
        Task<bool> LogoutAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly ApiService _apiService;

        public AuthService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginDto loginDto)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"[AuthService] Tentando login com email: {loginDto.Email}");

                var response = await _apiService.PostRawAsync("auth/login", loginDto);

                System.Diagnostics.Debug.WriteLine($"[AuthService] Status Code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetail = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"[AuthService] API retornou erro {response.StatusCode}: {errorDetail}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var loginResponse = JsonSerializer.Deserialize<LoginResponseDTO>(responseContent, options);

                if (loginResponse == null || string.IsNullOrEmpty(loginResponse.Token))
                {
                    System.Diagnostics.Debug.WriteLine("[AuthService] ERRO: Token ausente na resposta da API!");
                    return null;
                }

                System.Diagnostics.Debug.WriteLine($"[AuthService] Login OK! Role: {loginResponse.Role}");
                return loginResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AuthService] EXCEÇÃO NO LOGIN: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[AuthService] Comunicando logout à API...");

                // Chama o endpoint de logout
                // O ApiService injeta automaticamente o Header Authorization com o Token
                var response = await _apiService.PostAsync("auth/logout", new { });

                return response;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AuthService] EXCEÇÃO NO LOGOUT: {ex.Message}");
                return false;
            }
        }
    }
}