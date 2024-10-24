using AwesomeGIC.Bank.Domain.Entities;

namespace AwesomeGIC.Bank.Domain.Repositories
{
    public interface IBankAccountRepository
    {
        public Task<BankAccount?> GetByBankAccountIdAsync(string bankAccountId);

        public void Add(BankAccount bankAccount);
    }
}
