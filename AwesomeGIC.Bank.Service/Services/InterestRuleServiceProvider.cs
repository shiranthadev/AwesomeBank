using AwesomeGIC.Bank.Domain;
using AwesomeGIC.Bank.Domain.Repositories;
using AwesomeGIC.Bank.Dto.Common;
using AwesomeGIC.Bank.Dto.Rule;
using AwesomeGIC.Bank.Service.Handlers;
using AwesomeGIC.Bank.Service.Mappers;

namespace AwesomeGIC.Bank.Service.Services
{
    public class InterestRuleServiceProvider
    {
        private readonly IInterestRuleRepository _ruleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public InterestRuleServiceProvider(IInterestRuleRepository ruleRepository, IUnitOfWork unitOfWork)
        {
            _ruleRepository = ruleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> ProcessRule(CreateInterestRuleDto createRequest)
        {
            var result = await InterestRuleValidator.IsCreateRuleRequestValid(createRequest, _ruleRepository);

            if(!result.IsSuccess)
            {
                return result;
            }

            var newInterestRule = InterestRuleMapper.Map(createRequest);

            _ruleRepository.RemoveByDate(createRequest.RuleDate);
            _ruleRepository.Add(newInterestRule);

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error occured while saving interest rule. {ex.Message}");
            }
        }

        public async Task<InterestRuleDto[]> GetAllInterestRules()
        { 
            var entities = await _ruleRepository.GetAllAsync();
            var dtos = entities.Select(t => InterestRuleMapper.Map(t)).ToArray();

            return dtos;
        }
    }
}
