using AwesomeGIC.Bank.Dto.Transaction;
using AwesomeGIC.Bank.Service.Services;

namespace AwesomeGIC.Bank.Presentation.Menus
{
    public class PrintStatementMenu : BaseMenu
    {
        private const string _yearMonthFormat = "yyyyMM";
        private const string _dateFormat = Constants.InputDateFormat;
        private readonly TransactionServiceProvider _transactionService;

        public PrintStatementMenu(TransactionServiceProvider transactionService)
        {
            _transactionService = transactionService;
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

                /* Try to parse the user input into a  transaction dto object */
                if (!TryParse(userInput, out var filterRequest))
                {
                    continue;
                }

                await DisplayAccountStatementAsync(filterRequest!);

                PrintLine("\nPresee any key to continue");
                Console.Read();

                return;
            }
        }

        protected override void DisplayMenu()
        {
            PrintLine("\nPlease enter account and month to generate the statement <Account> <Year><Month>.");
            PrintLine("(or enter blank to go back to main menu): ");
        }

        private static bool TryParse(string userInput, out FilterAccountTxnDto? filterRequest)
        {
            filterRequest = null;

            var accountIdIndex = 0;
            var txnDateIndex = 1;
            var splits = userInput.Split(' ');

            if (splits.Length != 2)
            {
                PrintLine("Invalid account statement request format.");
            }
            else if (string.IsNullOrEmpty(splits[accountIdIndex]) || string.IsNullOrWhiteSpace(splits[accountIdIndex]))
            {
                PrintLine("Account Id cannot be empty.");
            }
            else if (string.IsNullOrEmpty(splits[txnDateIndex]) || string.IsNullOrWhiteSpace(splits[txnDateIndex]))
            {
                PrintLine("Transaction date cannot be empty.");
            }
            else if (!DateOnly.TryParseExact(splits[txnDateIndex], _yearMonthFormat, out DateOnly txnDate))
            {
                PrintLine($"Transaction date must be in format: {_yearMonthFormat.ToUpper()}.");
            }
            else
            {
                filterRequest = new FilterAccountTxnDto
                {
                    AccountId = splits[accountIdIndex],
                    TxnYear = txnDate.Year,
                    TxnMonth = txnDate.Month
                };

                return true;
            }

            return false;
        }

        private async Task DisplayAccountStatementAsync(FilterAccountTxnDto filterRequest)
        {
            var accountStatementItems = await _transactionService.GetAccountStatementItemsAsync(filterRequest);

            PrintLine("\n| Date     | Txn Id      | Type | Amount | Balance |");

            foreach (var item in accountStatementItems)
            {
                var tempSeqId = string.IsNullOrEmpty(item.TxnSequenceId) ? string.Empty.PadLeft(11, ' ') : item.TxnSequenceId;
                PrintLine($"| {item.TxnDate.ToString(_dateFormat)} | {tempSeqId} | {item.TxnType}    | {item.TxnAmount} | {item.Balance} |");
            }
        }
    }
}
