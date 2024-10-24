using AwesomeGIC.Bank.Dto.Rule;
using AwesomeGIC.Bank.Service.Services;

namespace AwesomeGIC.Bank.Presentation.Menus
{
    public class DefineInterestRulesMenu : BaseMenu
    {
        private const string _dateFormat = Constants.InputDateFormat;
        private readonly InterestRuleServiceProvider _interestRuleService;

        public DefineInterestRulesMenu(InterestRuleServiceProvider interestRuleService)
        {
            _interestRuleService = interestRuleService;
        }

        public override async Task ProcessUserInputAsync()
        {
            ClearConsole();

            while (true)
            {
                DisplayMenu();

                var userInput = Console.ReadLine();

                /* Exiting from Input Transaction Menu since the user entered nothing.*/
                if (string.IsNullOrEmpty(userInput))
                {
                    return;
                }

                /* Try to parse the user input into a interest rule dto object */
                if (TryParse(userInput, out var transactionDto))
                {
                    var result = await _interestRuleService.ProcessRule(transactionDto!);

                    if (result.IsSuccess)
                    {
                        PrintLine("Interest rule added.");
                        await DisplayInterestRulesAsync();

                        PrintLine("\nPresee any key to continue");
                        Console.Read();

                        return;
                    }
                    else
                    {
                        PrintLine(result.Message);
                    }
                }
            }
        }

        protected override void DisplayMenu()
        {
            PrintLine("\nPlease enter interest rules details in <Date> <RuleId> <Rate in %> format.");
            PrintLine("(or enter blank to go back to main menu): ");
        }

        private static bool TryParse(string userInput, out CreateInterestRuleDto? interestRule)
        {
            interestRule = null;
            var splits = userInput.Split(' ');

            if (splits.Length != 3)
            {
                PrintLine("Invalid interest rule input format.");
            }
            else if (!DateOnly.TryParseExact(splits[0], _dateFormat, out DateOnly ruleDate))
            {
                PrintLine($"Interest rule date must be in format: {_dateFormat.ToUpper()}.");
            }
            else if (string.IsNullOrEmpty(splits[1]) || string.IsNullOrWhiteSpace(splits[1]))
            {
                PrintLine("Interest rule Id cannot be empty.");
            }
            else if (string.IsNullOrEmpty(splits[2]) || string.IsNullOrWhiteSpace(splits[2]))
            {
                PrintLine($"Interest rule's interest rate cannot be empty.");
            }
            else if (!decimal.TryParse(splits[2], out decimal interestRate))
            {
                PrintLine($"Transaction amount must be a decimal value.");
            }
            else
            {
                interestRule = new CreateInterestRuleDto
                {
                    Id = splits[1],
                    RuleDate = ruleDate,
                    InterestRate = interestRate
                };

                return true;
            }

            return false;
        }

        private async Task DisplayInterestRulesAsync()
        { 
            var interestRules = await _interestRuleService.GetAllInterestRules();

            PrintLine("\n| Date     | RuleId | Rate (%) |");

            foreach (var interestRule in interestRules)
            {
                var paddedInterestRate = interestRule.InterestRate.ToString().PadLeft(8, ' ')[^8..];
                PrintLine($"| {interestRule.RuleDate.ToString(_dateFormat)} | {interestRule.Id} | {paddedInterestRate} |");
            }
        }
    }
}
