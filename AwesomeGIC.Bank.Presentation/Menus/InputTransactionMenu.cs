using AwesomeGIC.Bank.Dto.Enums;
using AwesomeGIC.Bank.Dto.Transaction;
using AwesomeGIC.Bank.Service.Services;

namespace AwesomeGIC.Bank.Presentation.Menus
{
    public class InputTransactionMenu : BaseMenu
    {
        private const string _dateFormat = Constants.InputDateFormat;
        private readonly TransactionServiceProvider _transactionService;

        public InputTransactionMenu(TransactionServiceProvider transactionService)
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

                /* Try to parse the user input into a transaction dto object */
                if (TryParse(userInput, out var accountTxn))
                {
                    await ProcessTransaction(accountTxn!);
                    await DisplayAccountTxnsForAccountAsync(accountTxn!.AccountId);

                    PrintLine("\nPresee any key to continue");
                    Console.Read();

                    return;
                }
            }
        }

        protected override void DisplayMenu()
        {
            PrintLine("\nPlease enter transaction details in <Date> <Account> <Type> <Amount> format.");
            PrintLine("(or enter blank to go back to main menu): ");
        }

        private static bool TryParse(string userInput, out CreateAccountTxnDto? accountTxn)
        {
            accountTxn = null;
            var splits = userInput.Split(' ');

            if (splits.Length != 4)
            {
                PrintLine("Invalid transaction input format.");
            }
            else if (!DateOnly.TryParseExact(splits[0], _dateFormat, out DateOnly transactionDate))
            {
                PrintLine($"Transaction date must be in format: {_dateFormat.ToUpper()}.");
            }
            else if (string.IsNullOrEmpty(splits[1]) || string.IsNullOrWhiteSpace(splits[1]))
            {
                PrintLine("Account Id cannot be empty.");
            }
            else if (string.IsNullOrEmpty(splits[2]) || string.IsNullOrWhiteSpace(splits[2]))
            {
                PrintLine($"Transaction Type cannot be empty.");
            }
            else if (!TransactionTypeExtensions.TryParse(splits[2][0], out TransactionType? transactionType))
            {
                PrintLine($"Invalid Transaction Type. It must be either: {TransactionType.Deposit.Code()} or {TransactionType.Withdraw.Code()}");
            }
            else if (string.IsNullOrEmpty(splits[3]) || string.IsNullOrWhiteSpace(splits[3]))
            {
                PrintLine($"Transaction amount cannot be empty.");
            }
            else if (!decimal.TryParse(splits[3], out decimal transactionAmount))
            {
                PrintLine($"Transaction amount must be a decimal value.");
            }
            else
            {
                accountTxn = new CreateAccountTxnDto
                {
                    TxnDate = transactionDate,
                    AccountId = splits[1],
                    TxnType = transactionType!.Value,
                    TxnAmount = transactionAmount
                };

                return true;
            }

            return false;
        }

        private async Task ProcessTransaction(CreateAccountTxnDto createRequest)
        {
            var result = await _transactionService.ProcessTransaction(createRequest);

            if (result.IsSuccess)
            {
                PrintLine("Transaction added.");
            }
            else
            {
                PrintLine(result.Message);
            }
        }

        private async Task DisplayAccountTxnsForAccountAsync(string accountId)
        {
            var acountTxns = await _transactionService.GetAccountTxnForAccountIdAsync(accountId);

            PrintLine("\n| Date     | Txn Id      | Type | Amount |");

            foreach (var item in acountTxns)
            {
                PrintLine($"| {item.TxnDate.ToString(_dateFormat)} | {item.TxnSequenceId} | {item.TxnType}    | {item.TxnAmount} |");
            }
        }
    }
}
