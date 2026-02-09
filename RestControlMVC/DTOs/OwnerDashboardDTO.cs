using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RestControlMVC.DTOs.Owner
{
    // DTO para o Dashboard (espelha o da API)
    public class OwnerDashboardDTO
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int TotalReservations { get; set; }
        public int PendingReservations { get; set; }
        public int TotalReviews { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalProducts { get; set; }
    }

    // DTO para edição do restaurante (apenas campos permitidos para Owner)
    public class RestaurantEditDTO
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres")]
        [Display(Name = "Descrição")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [Phone(ErrorMessage = "Telefone inválido")]
        [Display(Name = "Telefone")]
        public string Phone { get; set; } = string.Empty;
    }


    // DTO para reservas
    public class ReservationDTO
    {
        public int Id { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public int NumberOfPeople { get; set; }
        public string Status { get; set; } = "Pending";

        [JsonPropertyName("isReviewed")]
        public bool IsReviewed { get; set; }
    }
}