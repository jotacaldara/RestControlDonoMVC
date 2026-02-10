using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RestControlMVC.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("Token")?.Value;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            AddAuthorizationHeader();
            var requestUrl = $"{_httpClient.BaseAddress}{endpoint}";
            System.Diagnostics.Debug.WriteLine($"[ApiService] Chamando URL: {requestUrl}");

            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"API ERROR {response.StatusCode}: {content}");
                    return default;
                }

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<T>(content, options);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DESERIALIZATION ERROR: {ex.Message}");
                return default;
            }
        }

        // POST com autenticação (retorna objeto deserializado)
        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            return await HandleResponse<T>(response);
        }

        // POST que retorna bool (para operações simples)
        public async Task<bool> PostAsync(string endpoint, object data)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            return response.IsSuccessStatusCode;
        }

        //  MÉTODO PARA LOGIN
        // Retorna o HttpResponseMessage completo (usado no AuthService)
        public async Task<HttpResponseMessage> PostRawAsync(string endpoint, object data)
        {
            // NÃO adiciona Authorization header (login não precisa estar autenticado)
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            return response; // Retorna a resposta completa
        }

        // PUT com autenticação (retorna objeto)
        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            return await HandleResponse<T>(response);
        }

        // PUT que retorna bool
        public async Task<bool> PutAsync(string endpoint, object data)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(endpoint, content);
            return response.IsSuccessStatusCode;
        }

        // DELETE com autenticação
        public async Task<bool> DeleteAsync(string endpoint)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync(endpoint);
            return response.IsSuccessStatusCode;
        }

        // Método auxiliar para processar respostas
        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"API ERROR {response.StatusCode}: {errorContent}");
                return default;
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                return default;

            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}