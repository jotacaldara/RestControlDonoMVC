namespace RestControlMVC.DTOs
{
    public class DashboardKpiDTO
    {
        public int TotalRestaurants { get; set; }
        public int TotalReservations { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingApprovals { get; set; }
    }
}
