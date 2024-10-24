namespace AwesomeGIC.Bank.Service.Handlers
{
    public static class TransactionIdGenerator
    {
        private const string _dateForamt = "yyyyMMdd";
        private const char _zero = '0';

        public static string GetNextTxnId(DateOnly txnDate, int txnCountOnTxnDate)
        {
            var prefix = txnDate.ToString(_dateForamt);
            var paddedSuffix = (txnCountOnTxnDate + 1).ToString();
            var paddingSize = paddedSuffix.Length <= 2 ? 2 : paddedSuffix.Length;

            paddedSuffix = paddedSuffix.PadLeft(paddingSize, _zero);
            var transactionId = $"{prefix}-{paddedSuffix[^paddingSize..]}";

            return transactionId;
        }
    }
}
