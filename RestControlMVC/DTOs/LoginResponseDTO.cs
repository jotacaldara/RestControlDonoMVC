namespace RestControlMVC.DTOs
{
    public class LoginResponseDTO
    {
            public int UserId { get; set; }
            public string Name { get; set; } // Deve ser 'Name' pois a API envia 'Name'
            public string Role { get; set; }
      
}
}
