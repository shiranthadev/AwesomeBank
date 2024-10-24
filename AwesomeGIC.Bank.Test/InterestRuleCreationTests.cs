using AwesomeGIC.Bank.Domain;
using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Domain.Repositories;
using AwesomeGIC.Bank.Dto.Rule;
using AwesomeGIC.Bank.Service.Services;
using Moq;

namespace AwesomeGIC.Bank.Test
{
    public class InterestRuleCreationTests
    {
        private Mock<IInterestRuleRepository> _interestRuleRepositoryMock = new();
        private Mock<IUnitOfWork> _unitOfWorkMock = new();

        [Fact]
        public async Task WhenInterestRule_IsValid_RepositorysCalled()
        {
            // Arrange
            var createRequest = new CreateInterestRuleDto
            {
                Id = "RULE01",
                RuleDate = new DateOnly(2024, 3, 21),
                InterestRate = 3.4M
            };

            _interestRuleRepositoryMock
                .Setup(t => t.RemoveByDate(It.Is<DateOnly>(t => t == createRequest.RuleDate)));

            _interestRuleRepositoryMock
                .Setup(t => t.Add(It.Is<InterestRule>(t => t.Id == createRequest.Id &&
                                                           t.RuleDate == createRequest.RuleDate &&
                                                           t.InterestRate == createRequest.InterestRate)));

            _unitOfWorkMock
                .Setup(t => t.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var interestRuleService = new InterestRuleServiceProvider(_interestRuleRepositoryMock.Object,
                _unitOfWorkMock.Object);

            await interestRuleService.ProcessRule(createRequest);

            // Assert
            _interestRuleRepositoryMock.VerifyAll();
            _unitOfWorkMock.Verify(t => t.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(1));
        }

    }
}
