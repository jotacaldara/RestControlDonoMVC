using RestControlMVC.DTOs;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace RestControlMVC.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginDto loginDto);
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
                // 1. Serializar o objeto para JSON
                var jsonContent = JsonSerializer.Serialize(new
                {
                    email = loginDto.Email,
                    password = loginDto.Password
                });

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // 2. Fazer POST para a API
                var response = await _apiService.PostRawAsync("api/auth/login", content);

                // 3. Se não foi bem-sucedido, retorna null
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Login falhou: {response.StatusCode} - {errorContent}");
                    return null;
                }

                // 4. Ler o JSON de resposta
                var responseContent = await response.Content.ReadAsStringAsync();

                // 5. Desserializar para LoginResponseDto
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var loginResponse = JsonSerializer.Deserialize<LoginResponseDTO>(responseContent, options);

                return loginResponse;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro no AuthService.LoginAsync: {ex.Message}");
                return null;
            }
        }
    }
}