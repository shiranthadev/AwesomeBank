using AwesomeGIC.Bank.Domain;
using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Domain.Repositories;
using AwesomeGIC.Bank.Dto.Common;
using AwesomeGIC.Bank.Dto.Enums;
using AwesomeGIC.Bank.Dto.Transaction;
using AwesomeGIC.Bank.Service.Handlers;
using AwesomeGIC.Bank.Service.Mappers;

namespace AwesomeGIC.Bank.Service.Services
{
    public class TransactionServiceProvider
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionServiceProvider(
            IBankAccountRepository bankAccountRepository,
            ITransactionRepository transactionRepository,
            IUnitOfWork unitOfWork)
        {
            _bankAccountRepository = bankAccountRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> ProcessTransaction(CreateAccountTxnDto createRequest)
        {
            /* Validating create rransacton request. */
            var validationResult = TransactionValidator.IsCreateTxnRequestValid(createRequest);

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            var bankAccount = await _bankAccountRepository.GetByBankAccountIdAsync(createRequest.AccountId);

            /* If the transaction is a withdrawal checking if it is valid. */
            if (createRequest.TxnType == TransactionType.Withdraw)
            {
                validationResult = TransactionValidator.IsTransactionValidForWithdrawal(bankAccount, createRequest.TxnAmount);
            }

            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }

            try
            {
                await SaveTransactionAsync(bankAccount, createRequest);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error occured while saving transaction. {ex.Message}");
            }

            return Result.Success();
        }

        public async Task<AccountStatementItemDto[]> GetAccountStatementItemsAsync(FilterAccountTxnDto filterRequest)
        {
            return await _transactionRepository.GetAccountStatementItemsAsync(filterRequest.AccountId, filterRequest.TxnYear, filterRequest.TxnMonth);
        }

        public async Task<AccountTxnDto[]> GetAccountTxnForAccountIdAsync(string accountId)
        {
            var accountTxns = await _transactionRepository.GetByAccountId(accountId);
            return accountTxns.Select(AccountTxnMapper.MapAccountTxn).ToArray();
        }

        private async Task SaveTransactionAsync(BankAccount? bankAccount, CreateAccountTxnDto createRequest)
        {
            if (bankAccount == null)
            {
                bankAccount = new BankAccount { Id = createRequest.AccountId };
                _bankAccountRepository.Add(bankAccount);
            }

            if (createRequest.TxnType == TransactionType.Deposit)
            {
                bankAccount.Balance += createRequest.TxnAmount;
            }
            else if (createRequest.TxnType == TransactionType.Withdraw)
            {
                bankAccount.Balance -= createRequest.TxnAmount;
            }

            var txnCountOnTxnDate = await _transactionRepository.GetTxnCount(createRequest.AccountId, createRequest.TxnDate);
            var transactionId = TransactionIdGenerator.GetNextTxnId(createRequest.TxnDate, txnCountOnTxnDate);
            var accountTxn = AccountTxnMapper.MapToAccountTxnEntity(createRequest, transactionId);

            _transactionRepository.Add(accountTxn);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
