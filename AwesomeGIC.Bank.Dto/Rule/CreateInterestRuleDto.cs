using System.ComponentModel.DataAnnotations;

namespace AwesomeGIC.Bank.Dto.Rule
{
    public record CreateInterestRuleDto
    {
        public required string Id { get; init; }

        public required DateOnly RuleDate { get; init; }

        [Range(0, 100)]
        public required decimal InterestRate { get; init; }
    }
}
