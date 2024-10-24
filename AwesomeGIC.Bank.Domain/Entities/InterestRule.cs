using System.ComponentModel.DataAnnotations;

namespace AwesomeGIC.Bank.Domain.Entities
{
    public class InterestRule
    {
        [Key]
        public required string Id { get; set; }

        public required DateOnly RuleDate { get; set; }

        [Range(0, 100)]
        public required decimal InterestRate { get; set; }
    }
}
