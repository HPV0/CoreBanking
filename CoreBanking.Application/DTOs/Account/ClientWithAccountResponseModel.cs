namespace CoreBanking.Application.DTOs.Account
{
    public class ClientWithAccountResponseModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public AccountResponseModel AccountResponseModel { get; set; }
    }
}
