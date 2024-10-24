using AwesomeGIC.Bank.Dto.Enums;

namespace AwesomeGIC.Bank.Dto.Transaction
{
    public record CreateAccountTxnDto
    {
        public required DateOnly TxnDate { get; init; }

        public required string AccountId { get; init; }

        public required TransactionType TxnType { get; init; }

        public required decimal TxnAmount { get; init; }
    }
}
