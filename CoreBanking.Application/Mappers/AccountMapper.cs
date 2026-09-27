using CoreBanking.Application.DTOs.Account;
using CoreBanking.Application.DTOs.Client.CreateClient;
using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.Mappers
{
    [Mapper]
    public partial class AccountMapper
    {
        [MapProperty(nameof(Account.Balance.Amount), nameof(AccountResponseModel.Amount))]
        [MapProperty(nameof(Account.Currency.CurrencyCode), nameof(AccountResponseModel.CurrencyCode))]
        public partial AccountResponseModel AccountToAccountResponseModel(Account account);
        public partial List<AccountResponseModel> AccountsToAccountResponseModels(List<Account> accounts);

        //public partial void UpdateProductFromDTO(UpdateProductRequestDTO dto, Product product);
    }
}
