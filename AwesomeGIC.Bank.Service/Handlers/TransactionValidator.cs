
using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Dto.Common;
using AwesomeGIC.Bank.Dto.Transaction;

namespace AwesomeGIC.Bank.Service.Handlers
{
    public sealed class TransactionValidator
    {
        private const int _decimalPoints = 2;

        public static Result IsCreateTxnRequestValid(CreateAccountTxnDto request)
        {
            if (request.TxnAmount <= 0)
            {
                return Result.Failure("Transaction amount must be greater than zero.");
            }

            if (string.IsNullOrEmpty(request.AccountId))
            {
                return Result.Failure("Account is not specified.");
            }

            if (IsDecimalHaveExtraDecimalPoints(request.TxnAmount))
            {
                return Result.Failure($"Transaction amount cannot exceed {_decimalPoints} decimal places.");
            }

            return Result.Success();
        }

        public static Result IsTransactionValidForWithdrawal(BankAccount? bankAccount, decimal transactionAmount)
        {
            if (bankAccount == null)
            {
                return Result.Failure("First transaction on the account cannot be a withdrawal.");
            }

            if (bankAccount.Balance < transactionAmount)
            {
                return Result.Failure($"Not enough balance. Available balance is: {bankAccount.Balance}.");
            }

            return Result.Success();
        }

        private static bool IsDecimalHaveExtraDecimalPoints(decimal transactionAmount)
        {
            var splits = transactionAmount.ToString().Split('.');
            return splits.Length == 2 && int.Parse(splits[1]) > 100;
        }
    }
}
