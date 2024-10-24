namespace AwesomeGIC.Bank.Dto.Transaction
{
    public record AccountTxnDto
    {
        public required string TxnSequenceId { get; set; }

        public required DateOnly TxnDate { get; set; }

        public required string BankAccountId { get; set; }

        public required char TxnType { get; set; }

        public required decimal TxnAmount { get; set; }
    }
}
