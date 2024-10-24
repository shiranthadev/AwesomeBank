using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Domain.Repositories;
using AwesomeGIC.Bank.Dto.Transaction;
using Microsoft.EntityFrameworkCore;

namespace AwesomeGIC.Bank.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(AccountTxn accountTransaction)
        {
            _dbContext.AccountTxns.Add(accountTransaction);
        }

        public Task<AccountTxn[]> GetByAccountId(string accountId)
        {
            return _dbContext.AccountTxns
                             .AsNoTracking()
                             .Where(t => t.BankAccountId == accountId)
                             .OrderBy(t => t.TxnDate)
                             .ToArrayAsync();
        }

        public Task<int> GetTxnCount(string accountId, DateOnly txnDate)
        {
            return _dbContext.AccountTxns
                             .CountAsync(t => t.BankAccountId == accountId && t.TxnDate == txnDate);
        }

        public async Task<AccountStatementItemDto[]> GetAccountStatementItemsAsync(string accountId, int year, int month)
        {
            var statementItems = new List<AccountStatementItemDto>();
            var query = _dbContext.AccountTxns
                                  .Where(t => t.BankAccountId == accountId &&
                                              t.TxnDate.Year == year &&
                                              t.TxnDate.Month == month)
                                  .Select(t => new
                                  {
                                      TxnDate = t.TxnDate,
                                      TxnSequenceId = t.TxnSequenceId,
                                      TxnType = t.TxnType,
                                      TxnAmount = t.TxnAmount,
                                      EodBalance = _dbContext.AccountTxns
                                                             .Where(u => u.BankAccountId == accountId &&
                                                                         u.TxnDate <= t.TxnDate &&
                                                                         u.Id <= t.Id)
                                                             .Select(u => (double)u.TxnAmount * (u.TxnType == 'D' ? 1 : -1))
                                                             .Sum(),

                                      InterestRate = _dbContext.InterestRules
                                                               .Where(i => i.RuleDate <= t.TxnDate)
                                                               .OrderByDescending(i => i.RuleDate)
                                                               .Select(t => t.InterestRate)
                                                               .FirstOrDefault()
                                  }).OrderBy(t => t.TxnDate);

            var accountTxns = await query.ToListAsync();

            if (!accountTxns.Any())
            {
                return statementItems.ToArray();
            }

            var interestRules = await _dbContext.InterestRules
                                                .Where(t => t.RuleDate.Year == year && t.RuleDate.Month == month)
                                                .ToListAsync();

            var lastDayOfStatementMonth = LastDayOfMonth(new DateOnly(year,month, 1));
            var nextRuleDate = lastDayOfStatementMonth;
            var totalAnnualInterest = 0M;

            foreach (var accountTxn in accountTxns)
            {
                var daysCount = 0;
                var nextRule = interestRules.Where(t => t.RuleDate > accountTxn.TxnDate)
                                            .OrderBy(t => t.RuleDate)
                                            .FirstOrDefault();

                if (nextRule == null)
                {
                    daysCount = GetDaysGap(nextRuleDate, accountTxn.TxnDate);
                    nextRuleDate = lastDayOfStatementMonth;
                }
                else
                {
                    nextRuleDate = nextRule.RuleDate;
                    daysCount = GetDaysGap(accountTxn.TxnDate, nextRule.RuleDate);
                }

                statementItems.Add(new AccountStatementItemDto 
                { 
                    TxnDate = accountTxn.TxnDate,
                    TxnSequenceId = accountTxn.TxnSequenceId,
                    TxnType = accountTxn.TxnType,
                    TxnAmount = accountTxn.TxnAmount,
                    Balance =   (decimal)accountTxn.EodBalance
                });

                totalAnnualInterest += CalculateAnnualInterest((decimal)accountTxn.EodBalance, accountTxn.InterestRate, daysCount);
            }

            var interestForMonth = CalculateInterestForMonth(totalAnnualInterest);

            statementItems.Add(new AccountStatementItemDto
            {
                TxnDate = lastDayOfStatementMonth,
                TxnSequenceId = string.Empty,
                TxnType = 'I',
                TxnAmount = interestForMonth,
                Balance = statementItems.Last().Balance + interestForMonth
            });

            return statementItems.ToArray();
        }

        private static decimal CalculateAnnualInterest(decimal eodBalance, decimal interestRate, int daysCount)
        {
            return eodBalance * (interestRate/100M) * daysCount;
        }

        private static decimal CalculateInterestForMonth(decimal totalAnnualInterest)
        {
            return Math.Round(totalAnnualInterest / 365M, 2);
        }

        private static int GetDaysGap(DateOnly date1, DateOnly date2)
        {
            return Math.Abs(date1.DayNumber - date2.DayNumber);
        }

        private static DateOnly LastDayOfMonth(DateOnly dateOnly)
        {
            return dateOnly.AddMonths(1).AddDays(-1);
        }
    }
}
