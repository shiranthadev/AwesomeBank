using System.ComponentModel.DataAnnotations;

namespace AwesomeGIC.Bank.Domain.Entities
{
    public class AccountTxn
    {
        [Key]
        public int Id { get; set; }

        public required string TxnSequenceId { get; set; }

        public required DateOnly TxnDate { get; set; }

        public required string BankAccountId { get; set; }

        public required char TxnType { get; set; }

        public required decimal TxnAmount { get; set; }

        public BankAccount BankAccount { get; set; } = null!;
    }

}
