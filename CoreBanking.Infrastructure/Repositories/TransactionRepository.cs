using CoreBanking.Application.DTOs.Transaction;
using CoreBanking.Application.Interfaces.IRepositories;
using CoreBanking.Domain.Constants;
using CoreBanking.Domain.Entities.TransactionEntities;
using CoreBanking.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CoreBanking.Infrastructure.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository
    {
        protected override IQueryable<Transaction> BaseQuery
            => _dbContext.Transactions;


        public TransactionRepository(ApplicationDbContext db) : base(db) { }

        public async Task<List<AccountTotalDepositesResponseModel>> GetAccountsTotalDepositBetweenDatesAsync(DateTime timeFrom, DateTime timeTo)
        {
            return await _dbContext.Database
                .SqlQuery<AccountTotalDepositesResponseModel>($"""
            SELECT
                a."id" AS "account_id",
                c."currency_code" AS "currency_code",
                COALESCE(
                    (
                        SELECT ROUND(
                            SUM(d."amount" * d."exchange_rate"),
                            {FinancialPrecision.AmountPrecision}
                        )
                        FROM "active_transaction" d
                        WHERE d."account_to_id" = a."id"
                          AND d."transaction_id" IN (
                              SELECT DISTINCT t."transaction_id"
                              FROM "active_transaction" t
                              WHERE t."inserted_at" >= {timeFrom}
                                AND t."inserted_at" < {timeTo}
                          )
                    ),
                    0::numeric
                ) AS "total_deposites"
            FROM "active_accounts" a
            INNER JOIN "active_currencies" c
                ON c."id" = a."currency_id";
            """)
                .ToListAsync();
        }

        public async Task<List<AccountTotalWithdrawalsResponseModel>> GetTotalWithdrawalsByAccountsAsync(DateTime timeFrom, DateTime timeTo)
        {
            return await _dbContext.Database
                .SqlQuery<AccountTotalWithdrawalsResponseModel>($"""
                    SELECT
                        a."id" AS "account_id",
                        c."currency_code" AS "currency_code",
                        COALESCE(
                            (
                                SELECT ROUND(
                                    SUM(d."amount"),
                                    {FinancialPrecision.AmountPrecision}
                                )
                                FROM "active_transaction" d
                                WHERE d."account_from_id" = a."id"
                                  AND d."transaction_id" IN (
                                      SELECT DISTINCT t."transaction_id"
                                      FROM "active_transaction" t
                                      WHERE t."inserted_at" >= {timeFrom}
                                        AND t."inserted_at" < {timeTo}
                                  )
                            ),
                            0::numeric
                        ) AS "total_withdrawals"
                    FROM "active_accounts" a
                    INNER JOIN "active_currencies" c
                        ON c."id" = a."currency_id";
                    """)
                .ToListAsync();
        }

        public async Task<List<Transaction>> GetByTransactionIdAsync(int id)
        {
            var transactionDetails = await BaseQuery.Where(transaction => transaction.TransactionId == id).ToListAsync();
            if (transactionDetails.Count == 0)
                return null;
            return transactionDetails;
        }

        public async Task<List<List<Transaction>>> GetAllTransactionsAsync()
        {
            List<List<Transaction>> groupedTransactions = await BaseQuery.GroupBy(t => t.TransactionId)
                .Select(g => g.ToList())
                .ToListAsync();

            return groupedTransactions;
        }

        public async Task<List<AccountCashFlowResponseModel>> GetTotalCashFlowsByAccountsAsync
            (DateTime timeFrom, DateTime timeTo)
        {
            return await _dbContext.Database
                .SqlQuery<AccountCashFlowResponseModel>($"""
                    WITH period_transactions AS (
                        SELECT DISTINCT "transaction_id"
                        FROM "active_transaction"
                        WHERE "inserted_at" >= {timeFrom}
                          AND "inserted_at" < {timeTo}
                    ),
                    account_movements AS (
                        -- Deposits
                        SELECT
                            d."account_to_id" AS "account_id",
                            SUM(d."amount" * d."exchange_rate") AS "incomes",
                            0::numeric AS "spending"
                        FROM "active_transaction" d
                        INNER JOIN period_transactions pt
                            ON pt."transaction_id" = d."transaction_id"
                        WHERE d."account_to_id" IS NOT NULL
                        GROUP BY d."account_to_id"

                        UNION ALL

                        -- Withdrawals
                        SELECT
                            d."account_from_id" AS "account_id",
                            0::numeric AS "incomes",
                            SUM(d."amount") AS "spending"
                        FROM "active_transaction" d
                        INNER JOIN period_transactions pt
                            ON pt."transaction_id" = d."transaction_id"
                        WHERE d."account_from_id" IS NOT NULL
                        GROUP BY d."account_from_id"
                    ),
                    account_totals AS (
                        SELECT
                            "account_id",
                            ROUND(SUM("incomes"), {FinancialPrecision.AmountPrecision}) AS "incomes",
                            ROUND(SUM("spending"), {FinancialPrecision.AmountPrecision}) AS "spending"
                        FROM account_movements
                        GROUP BY "account_id"
                    )
                    SELECT
                        a."id" AS "id",
                        c."currency_code" AS "currency_code",
                        COALESCE(at."incomes", 0::numeric) AS "incomes_amount",
                        COALESCE(at."spending", 0::numeric) AS "spending_amount",
                        COALESCE(at."incomes", 0::numeric)
                            - COALESCE(at."spending", 0::numeric)
                            AS "balance_change_amount"
                    FROM "active_accounts" a
                    INNER JOIN "active_currencies" c
                        ON c."id" = a."currency_id"
                    LEFT JOIN account_totals at
                        ON at."account_id" = a."id";
                """).ToListAsync();
        }
    }
}
