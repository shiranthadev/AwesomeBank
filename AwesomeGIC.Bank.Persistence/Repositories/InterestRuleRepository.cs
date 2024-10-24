using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AwesomeGIC.Bank.Persistence.Repositories
{
    public class InterestRuleRepository : IInterestRuleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public InterestRuleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsAlreadyExistsAsync(string ruleId)
        {
            return await _dbContext.InterestRules.AnyAsync(t => t.Id == ruleId);
        }

        public void Add(InterestRule interestRule)
        {
            _dbContext.InterestRules.Add(interestRule);
        }        

        public void RemoveByDate(DateOnly ruleDate)
        {
            _dbContext.InterestRules.RemoveRange(_dbContext.InterestRules.Where(t => t.RuleDate == ruleDate));
        }

        public async Task<List<InterestRule>> GetAllAsync()
        {
            return await _dbContext.InterestRules.ToListAsync();
        }
    }
}
