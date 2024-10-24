using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Dto.Enums;
using AwesomeGIC.Bank.Dto.Transaction;

namespace AwesomeGIC.Bank.Service.Mappers
{
    public static class AccountTxnMapper
    {
        public static AccountTxn MapToAccountTxnEntity(CreateAccountTxnDto request, string transactionId)
        {
            return new AccountTxn
            {
                TxnSequenceId = transactionId,
                TxnDate = request.TxnDate,
                BankAccountId = request.AccountId,
                TxnType = request.TxnType.Code(),
                TxnAmount = request.TxnAmount
            };
        }

        public static AccountTxnDto MapAccountTxn(AccountTxn accountTxn)
        {
            return new AccountTxnDto
            {
                TxnSequenceId = accountTxn.TxnSequenceId,
                TxnDate = accountTxn.TxnDate,
                BankAccountId = accountTxn.BankAccountId,
                TxnType = accountTxn.TxnType,
                TxnAmount = accountTxn.TxnAmount
            };
        }
    }
}
