using AwesomeGIC.Bank.Domain.Repositories;
using AwesomeGIC.Bank.Dto.Rule;
using AwesomeGIC.Bank.Service.Handlers;
using Moq;

namespace AwesomeGIC.Bank.Test
{
    public class InterestRuleValidatorTests
    {
        private Mock<IInterestRuleRepository> _interestRuleRepositoryMock = new();
        
        [Fact]
        public async Task WhenRuleId_IsEmpty_ValidationFails()
        {
            // Arrange
            var createRequest = new CreateInterestRuleDto
            {
                Id = string.Empty,//"RULE01"
                RuleDate = new DateOnly(2024, 3, 21),
                InterestRate = 3.4M
            };

            // Act
            var result = await InterestRuleValidator.IsCreateRuleRequestValid(createRequest, _interestRuleRepositoryMock.Object);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Interest rule id cannot empty.", result.Message);
        }

        [Fact]
        public async Task WhenInterest_IsNotBetween_0_And_100_ValidationFails()
        {
            // Arrange
            var createRequest = new CreateInterestRuleDto
            {
                Id = "RULE01",
                RuleDate = new DateOnly(2024, 3, 21),
                InterestRate = 120M
            };

            // Act
            var result = await InterestRuleValidator.IsCreateRuleRequestValid(createRequest, _interestRuleRepositoryMock.Object);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Interest rate must be between 0 and 100.", result.Message);
        }

        [Fact]
        public async Task WhenInterestRuleId_AlreadyExists_ValidationFails()
        {
            // Arrange
            var createRequest = new CreateInterestRuleDto
            {
                Id = "RULE01",
                RuleDate = new DateOnly(2024, 3, 21),
                InterestRate = 3.4M
            };

            _interestRuleRepositoryMock
                .Setup(t => t.IsAlreadyExistsAsync(It.Is<string>(t => t == createRequest.Id)))
                .Returns(Task.FromResult(true));

            // Act
            var result = await InterestRuleValidator.IsCreateRuleRequestValid(createRequest, _interestRuleRepositoryMock.Object);

            // Assert
            _interestRuleRepositoryMock.VerifyAll();

            Assert.False(result.IsSuccess);
            Assert.Equal($"Interest rule id: {createRequest.Id} already exists.", result.Message);
        }
    }
}
