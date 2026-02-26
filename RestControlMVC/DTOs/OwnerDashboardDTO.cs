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

        public decimal TotalRevenue { get; set; }
        public decimal TotalCommissions { get; set; }
        public decimal NetRevenue { get; set; }

        public SubscriptionDTO Subscription { get; set; }
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
        public int ReservationId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public int NumberOfPeople { get; set; }
        public string Status { get; set; } = "Pending";

        [JsonPropertyName("isReviewed")]
        public bool IsReviewed { get; set; }

        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

        public decimal TotalAmount { get; set; }
    }

    public class RevenueDTO
    { 
        public decimal TotalRevenue { get; set; }
        public decimal TotalCommissions { get; set; }
        public decimal NetRevenue { get; set; }
        public int TotalReservations { get; set; }
        public decimal AverageReservationValue { get; set; }
        public List<MonthlyRevenueDTO> MonthlyData { get; set; } = new();
    }

    public class MonthlyRevenueDTO
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public decimal Revenue { get; set; }
        public decimal Commissions { get; set; }
        public decimal Net { get; set; }
    }
}