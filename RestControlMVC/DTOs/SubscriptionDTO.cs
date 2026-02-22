namespace RestControlMVC.DTOs
{
    public class SubscriptionDTO
    {
        public int SubscriptionId { get; set; }
        public string RestaurantName { get; set; }
        public string PlanName { get; set; }
        public decimal monthlyPrice { get; set; }
        public decimal Commission { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int DaysRemaining => EndDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber;

        public bool IsExpiring { get; set; }
    }
}
