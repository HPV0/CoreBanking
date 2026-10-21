using CoreBanking.Application.DTOs.Account;
using CoreBanking.Domain.Entities.AccountEntities;
using Riok.Mapperly.Abstractions;

namespace CoreBanking.Application.Mappers
{
    [Mapper]
    public partial class AccountMapper
    {
        [MapProperty(nameof(Account.Balance.Amount), nameof(AccountResponseModel.Amount))]
        [MapProperty(nameof(Account.Currency.CurrencyCode), nameof(AccountResponseModel.CurrencyCode))]
        public partial AccountResponseModel ToAccountResponseModel(Account account);
        public partial List<AccountResponseModel> ToAccountResponseModels(List<Account> accounts);
    }
}
