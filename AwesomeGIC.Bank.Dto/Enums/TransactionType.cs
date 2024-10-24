namespace AwesomeGIC.Bank.Dto.Enums
{
    public enum TransactionType
    {
        Deposit,
        Withdraw
    }

    public static class TransactionTypeExtensions
    {
        private const char DepositCode = 'D';
        private const char WithdrawCode = 'W';

        public static bool TryParse(char code, out TransactionType? transactionType)
        {
            switch (code)
            {
                case DepositCode:
                    transactionType = TransactionType.Deposit;
                    break;

                case WithdrawCode:
                    transactionType = TransactionType.Withdraw;
                    break;

                default:
                    transactionType = null;
                    return false;
            }

            return true;
        }

        public static char Code(this TransactionType type)
        {
            switch (type)
            {
                case TransactionType.Deposit:
                    return DepositCode;

                case TransactionType.Withdraw:
                    return WithdrawCode;

                default:
                    throw new ArgumentException($"Invalid transaction type. There is no code mapped for transaction type: {type}", nameof(type));
            }
        }
    }
}
