using System.ComponentModel.DataAnnotations;

namespace RestControlMVC.DTOs
{
    public class LoginResponseDTO
    {
            public string Token { get; set; }      // ← JWT Token
            public int UserId { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }       // "Admin" ou "Owner"
            public int? RestaurantId { get; set; } // Apenas para Owners
        
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool RememberMe { get; set; }
    }
}
