using RestControlMVC.DTOs;

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

        public async Task<LoginResponseDTO> LoginAsync(LoginDto loginDto)
        {
            return await _apiService.PostAsync<LoginResponseDTO>("auth/login", loginDto);
        }
    }
}