using AwesomeGIC.Bank.Domain.Entities;

namespace AwesomeGIC.Bank.Domain.Repositories
{
    public interface IInterestRuleRepository
    {
        public Task<bool> IsAlreadyExistsAsync(string ruleId);

        public void Add(InterestRule interestRule);

        public void RemoveByDate(DateOnly ruleDate);

        public Task<List<InterestRule>> GetAllAsync();
    }
}
