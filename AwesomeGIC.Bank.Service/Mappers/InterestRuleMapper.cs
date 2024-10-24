using AwesomeGIC.Bank.Domain.Entities;
using AwesomeGIC.Bank.Dto.Rule;

namespace AwesomeGIC.Bank.Service.Mappers
{
    public static class InterestRuleMapper
    {
        public static InterestRule Map(CreateInterestRuleDto createRequest)
        {
            return new InterestRule
            {
                Id = createRequest.Id,
                RuleDate = createRequest.RuleDate,
                InterestRate = createRequest.InterestRate
            };
        }

        public static InterestRuleDto Map(InterestRule interestRule)
        {
            return new InterestRuleDto
            {
                Id = interestRule.Id,
                RuleDate = interestRule.RuleDate,
                InterestRate = interestRule.InterestRate
            };
        }
    }
}
