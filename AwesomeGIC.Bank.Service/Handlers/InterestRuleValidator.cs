
using AwesomeGIC.Bank.Domain.Repositories;
using AwesomeGIC.Bank.Dto.Common;
using AwesomeGIC.Bank.Dto.Rule;

namespace AwesomeGIC.Bank.Service.Handlers
{
    public static class InterestRuleValidator
    {
        public static async Task<Result> IsCreateRuleRequestValid(CreateInterestRuleDto createRequest, 
            IInterestRuleRepository _ruleRepository)
        {
            if (string.IsNullOrEmpty(createRequest.Id))
            {
                return Result.Failure("Interest rule id cannot empty.");
            }

            if (createRequest.InterestRate < 0 || createRequest.InterestRate > 100)
            {
                return Result.Failure("Interest rate must be between 0 and 100.");
            }

            if (await _ruleRepository.IsAlreadyExistsAsync(createRequest.Id))
            {
                return Result.Failure($"Interest rule id: {createRequest.Id} already exists.");
            }

            return Result.Success();
        }
    }
}
