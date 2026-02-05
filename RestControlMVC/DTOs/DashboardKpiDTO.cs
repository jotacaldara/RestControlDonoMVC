namespace RestControlMVC.DTOs
{
    public class DashboardKpiDTO
    {
        public int TotalRestaurants { get; set; }
        public int TotalReservations { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingApprovals { get; set; }

        public List<RevenueDataDTO> RevenueHistory { get; set; } = new();

    }

    public class RevenueDataDTO
    {
        public int Month { get; set; }
        public decimal Total { get; set; }
    }
}
