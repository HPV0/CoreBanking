namespace CoreBanking.Application.DTOs.Client
{
    public class UpdateClientRequest
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? PassportNumber { get; set; }
        public string? Email { get; set; }
    }
}
