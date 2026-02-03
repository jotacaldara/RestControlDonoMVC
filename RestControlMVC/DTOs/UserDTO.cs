namespace RestControlMVC.DTOs
{
    public class UserDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string role { get; set; }
        public bool isActive { get; set; }
    }
}
