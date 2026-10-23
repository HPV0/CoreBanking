using CoreBanking.Application.Exceptions;
using CoreBanking.Domain.Common;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities.AccountEntities;
using CoreBanking.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoreBanking.Domain.Entities.TransactionEntities
{
    public class Transaction : BaseEntity
    {
        private List<TransactionDetail> _details = new();
        
        public IReadOnlyCollection<TransactionDetail>? Details => _details?.ToList();
        
        private Transaction() { }
        
        private Transaction(Account sender, Account recevier, Account baseBankingAccount,
            decimal senderExchangeRate, decimal recevierExchangeRate,
            decimal amount) : base()
        {
            Money sendingMoney = Money.Create(amount, sender.Currency.CurrencyCode);

            if (sender.Id == recevier.Id)
                throw new UnprocessableException("Sender and Recevier accounts cannot be same.");

            if (sender.CurrencyId == BaseCurrency.Id && senderExchangeRate != 1)
                throw new UnprocessableException("Sender Exchange rate for base currency cannot be other then 1");

            if (recevier.CurrencyId == BaseCurrency.Id && recevierExchangeRate != 1)
                throw new UnprocessableException("Receiver Exchange rate for base currency cannot be other then 1");

            if (sender.CurrencyId == recevier.CurrencyId)
            {
                AddTransactionDetail(sender, recevier, 1, 1, sendingMoney, out _);
            }
            else
            {
                Money baseAccountRecevingMoney;
                AddTransactionDetail(sender, baseBankingAccount, senderExchangeRate, 1, sendingMoney, out baseAccountRecevingMoney);
                AddTransactionDetail(baseBankingAccount, recevier, 1, recevierExchangeRate, baseAccountRecevingMoney, out _);
            }

        }


        public static Transaction Create(Account sender, Account recevier, Account baseBankingAccount,
            decimal senderExchangeRate, decimal recevierExchangeRate,
            decimal amount)
        {
            return new Transaction(sender, recevier, baseBankingAccount,
            senderExchangeRate, recevierExchangeRate, amount);
        }


        private void AddTransactionDetail(Account accountFrom, Account accountTo,
            decimal senderExchangeRate, decimal recevierExchangeRate,
            Money money, out Money accountToMoney)
        {

            accountFrom.Withdraw(money);

            decimal finalExchangeRate;

            accountToMoney = money.Exchange(senderExchangeRate, recevierExchangeRate, accountTo.Currency.CurrencyCode, out finalExchangeRate);
            accountTo.Deposit(accountToMoney);

            TransactionDetail detail = TransactionDetail.Create(Id, accountFrom, accountTo, finalExchangeRate, money);
            
            _details.Add(detail);
        }

        public void GetTransactionDetailsSenderAndReceiver(out TransactionDetail receiverAccount, out TransactionDetail senderAccount) 
        {
            if (_details.Count == 0)
                throw new Exception("Cannot get Sender And Receiver, when Details count is 0");
            
            receiverAccount = _details[0];
            senderAccount = _details[0];

            foreach (var transactionDetail in _details)
            {
                if (transactionDetail.AccountFromId != BankAccount.Id)
                {
                    senderAccount = transactionDetail;
                }
                else if (transactionDetail.AccountToId != BankAccount.Id)
                {
                    receiverAccount = transactionDetail;
                }
                else {
                    throw new Exception("No AccountFromId or AccountToId in transactions");
                }
            }

        }

        //public void DeactivateTransaction(Account senderAccount, Account reciverAccount)
        //{
            
        //    foreach (var transactionDetail in _details)
        //    {
        //        Money sendingMoney, reciveingMoney;
        //        if (transactionDetail.AccountFromId == senderAccount.Id) {
        //            var sendingMoney = Money.Create(transactionDetail.Amount, senderAccount.Currency.CurrencyCode);  
        //            senderAccount.Deposit(sendingMoney);
        //        }

        //        if (transactionDetail.AccountToId == reciverAccount.Id) {
        //            var reciveingMoney = Money.Create(transactionDetail.Amount, senderAccount.Currency.CurrencyCode);
        //            senderAccount.Deposit(reciveingMoney);
        //        }
        //    }
        //}
    }
}
