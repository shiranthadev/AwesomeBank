using System.ComponentModel.DataAnnotations;

namespace AwesomeGIC.Bank.Domain.Entities
{
    public class BankAccount
    {
        [Key]
        public required string Id { get; set; }

        public decimal Balance { get; set; }

        public ICollection<AccountTxn> BankAccountTransactions { get; } = []; // Collection navigation containing dependents
    }
}
