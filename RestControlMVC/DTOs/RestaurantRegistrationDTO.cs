using System.ComponentModel.DataAnnotations;

namespace RestControlMVC.DTOs
{
    public class RestaurantRegistrationDTO
    {
        [Required(ErrorMessage = "Nome completo é obrigatório")]
        [StringLength(150)]
        [Display(Name = "Nome Completo")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Email")]
        public string OwnerEmail { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        [Phone(ErrorMessage = "Telefone inválido")]
        [Display(Name = "Telefone")]
        public string OwnerPhone { get; set; }

        [Required(ErrorMessage = "Password é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password deve ter entre 6 e 100 caracteres")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirme a password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As passwords não coincidem")]
        [Display(Name = "Confirmar Password")]
        public string ConfirmPassword { get; set; }

        // Dados do Restaurante
        [Required(ErrorMessage = "Nome do restaurante é obrigatório")]
        [StringLength(150)]
        [Display(Name = "Nome do Restaurante")]
        public string RestaurantName { get; set; }

        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(500, MinimumLength = 20)]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Morada é obrigatória")]
        [StringLength(200)]
        [Display(Name = "Morada Completa")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Cidade é obrigatória")]
        [StringLength(100)]
        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Required(ErrorMessage = "Telefone do restaurante é obrigatório")]
        [Phone(ErrorMessage = "Telefone inválido")]
        [Display(Name = "Telefone do Restaurante")]
        public string RestaurantPhone { get; set; }

        [Required(ErrorMessage = "Email do restaurante é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Display(Name = "Email do Restaurante")]


        public string RestaurantEmail { get; set; }

        [Required(ErrorMessage = "Por favor, escolha um plano")]
        [Range(1, int.MaxValue, ErrorMessage = "Por favor, escolha um plano")]
        [Display(Name = "Plano")]
        public int PlanId { get; set; }
    }
}
