using AwesomeGIC.Bank.Domain;
using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Domain.Repositories;
using AwesomeGIC.Bank.Dto.Transaction;
using AwesomeGIC.Bank.Service.Services;
using Moq;

namespace AwesomeGIC.Bank.Test
{
    public class TransactionCreationTests
    {
        private Mock<IBankAccountRepository> _bankAccountRepositoryMock = new();
        private Mock<ITransactionRepository> _txnRepositoryMock = new();
        private Mock<IUnitOfWork> _unitOfWorkMock = new();

        [Fact]
        public async Task WhenAccountTxn_IsValid_RepositorysCalled()
        {
            // Arrange
            var createRequest = new CreateAccountTxnDto
            {
                TxnDate = new DateOnly(2024, 9, 7),
                AccountId = "ACC01",
                TxnType = Dto.Enums.TransactionType.Deposit,
                TxnAmount = 100
            };

            _bankAccountRepositoryMock
                .Setup(t => t.GetByBankAccountIdAsync(It.Is<string>(t => t == createRequest.AccountId)))
                .Returns(Task.FromResult<BankAccount?>(null));

            _bankAccountRepositoryMock
                .Setup(t => t.Add(It.Is<BankAccount>(u => u.Id == createRequest.AccountId)));

            _txnRepositoryMock
                .Setup(t => t.GetTxnCount(It.Is<string>(u => u == createRequest.AccountId),
                                          It.Is<DateOnly>(u => u == createRequest.TxnDate)))
                .Returns(Task.FromResult<int>(0));

            _txnRepositoryMock
                .Setup(t => t.Add(It.Is<AccountTxn>(u => u.BankAccountId == createRequest.AccountId)));

            _unitOfWorkMock
                .Setup(t => t.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var txnServiceProvider = new TransactionServiceProvider(_bankAccountRepositoryMock.Object, 
                _txnRepositoryMock.Object, _unitOfWorkMock.Object);

            // Act
            await txnServiceProvider.ProcessTransaction(createRequest);

            // Assert
            _bankAccountRepositoryMock.VerifyAll();
            _txnRepositoryMock.VerifyAll();
            _unitOfWorkMock.Verify(t => t.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
        }
    }
}
