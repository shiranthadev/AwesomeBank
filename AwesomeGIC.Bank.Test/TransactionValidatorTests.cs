using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Dto.Transaction;
using AwesomeGIC.Bank.Service.Handlers;

namespace AwesomeGIC.Bank.Test
{
    public class TransactionValidatorTests
    {
        [Fact]
        public void WhenTxnAmount_LessThanZero_ValidationFail()
        {
            // Arrange
            var createRequest = new CreateAccountTxnDto
            {
                TxnDate = new DateOnly(2024, 1, 7),
                AccountId = "ACC01",
                TxnType = Dto.Enums.TransactionType.Withdraw,
                TxnAmount = -100
            };

            // Act
            var validationResult = TransactionValidator.IsCreateTxnRequestValid(createRequest);

            // Assert
            Assert.False(validationResult.IsSuccess);
            Assert.Equal("Transaction amount must be greater than zero.", validationResult.Message);
        }

        [Fact]
        public void WhenTxnAmount_HasMoreThan2DecimalPoints_ValidationFail()
        {
            // Arrange
            var createRequest = new CreateAccountTxnDto
            {
                TxnDate = new DateOnly(2024, 1, 7),
                AccountId = "ACC01",
                TxnType = Dto.Enums.TransactionType.Withdraw,
                TxnAmount = 100.123M
            };

            // Act
            var validationResult = TransactionValidator.IsCreateTxnRequestValid(createRequest);

            // Assert
            Assert.False(validationResult.IsSuccess);
            Assert.Equal("Transaction amount cannot exceed 2 decimal places.", validationResult.Message);
        }

        [Fact]
        public void WhenAccountId_NotSpecified_ValidationFail()
        {
            // Arrange
            var createRequest = new CreateAccountTxnDto
            {
                TxnDate = new DateOnly(2024, 1, 7),
                AccountId = string.Empty,
                TxnType = Dto.Enums.TransactionType.Withdraw,
                TxnAmount = 100
            };

            // Act
            var validationResult = TransactionValidator.IsCreateTxnRequestValid(createRequest);

            // Assert
            Assert.False(validationResult.IsSuccess);
            Assert.Equal("Account is not specified.", validationResult.Message);
        }

        [Fact]
        public void WhenBankAccount_NotExist_FirstTxnCannotBeWithdrawal()
        {
            // Act
            var validationResult = TransactionValidator.IsTransactionValidForWithdrawal(null, 100);

            // Assert
            Assert.False(validationResult.IsSuccess);
            Assert.Equal("First transaction on the account cannot be a withdrawal.", validationResult.Message);
        }

        [Fact]
        public void WhenWithdrawAmount_Cannot_ExceedBalanceAmount()
        {
            // Arrange
            var bankAccount = new BankAccount { Id = "A001", Balance = 200 };

            // Act
            var validationResult = TransactionValidator.IsTransactionValidForWithdrawal(bankAccount, 300);

            // Assert
            Assert.False(validationResult.IsSuccess);
            Assert.Equal($"Not enough balance. Available balance is: {bankAccount.Balance}.", validationResult.Message);
        }
    }
}