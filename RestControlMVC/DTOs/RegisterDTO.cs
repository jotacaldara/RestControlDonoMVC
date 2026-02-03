namespace RestControlMVC.DTOs
{
    public class RegisterDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Role { get; set; } // Adicione este campo para o formulário
    }
}
