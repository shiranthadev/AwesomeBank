namespace AwesomeGIC.Bank.Dto.Rule
{
    public class InterestRuleDto
    {
        public required string Id { get; init; }

        public required DateOnly RuleDate { get; init; }

        public required decimal InterestRate { get; init; }
    }
}
