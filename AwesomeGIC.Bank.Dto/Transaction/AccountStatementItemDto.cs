namespace AwesomeGIC.Bank.Dto.Transaction
{
    public record AccountStatementItemDto
    {
        public required string TxnSequenceId { get; init; }

        public required DateOnly TxnDate { get; init; }

        public required char TxnType { get; init; }

        public required decimal TxnAmount { get; init; }

        public required decimal Balance { get; init; }
    }
}
