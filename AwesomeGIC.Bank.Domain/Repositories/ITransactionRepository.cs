
using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Dto.Transaction;

namespace AwesomeGIC.Bank.Domain.Repositories
{
    public interface ITransactionRepository
    {
        public void Add(AccountTxn accountTransaction);

        public Task<AccountTxn[]> GetByAccountId(string accountId);

        public Task<int> GetTxnCount(string accountId, DateOnly txnDate);

        public Task<AccountStatementItemDto[]> GetAccountStatementItemsAsync(string accountId, int year, int month);
    }
}
