namespace CoreBanking.Application.DTOs.Client.CreateClient
{
    public class ClientResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PassportNumber { get; set; }
        public DateOnly? Birthday { get; set; }
    }
}
