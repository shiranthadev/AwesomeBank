namespace AwesomeGIC.Bank.Dto.Transaction
{
    public record FilterAccountTxnDto
    {
        public required string AccountId { get; set; }

        public required int TxnYear { get; set; }

        public required int TxnMonth { get; set; }
    }
}
