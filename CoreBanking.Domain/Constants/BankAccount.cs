using CoreBanking.Domain.Entities;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.ValueObjects.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace CoreBanking.Domain.Constants
{
    public static class BankAccount
    {
        public readonly static Guid Id = new Guid("9C7A52DF-668C-44A2-9DF0-DE71A98C8BD6");
        //public static Account Create()
        //{
        //    return Account.Create(Id, BaseCurrency.Create(), BankClient.Create());
        //}
    }
}
