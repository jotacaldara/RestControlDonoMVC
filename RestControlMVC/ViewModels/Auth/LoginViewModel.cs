using System.ComponentModel.DataAnnotations;

namespace RestControlMVC.ViewModels.Auth
{
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
