
namespace AwesomeGIC.Bank.Presentation.Menus
{
    public class MainMenu : BaseOptionMenu<string>
    {
        private readonly InputTransactionMenu _inputTransactionMenu;
        private readonly DefineInterestRulesMenu _defineInterestRulesMenu;
        private readonly PrintStatementMenu _printStatementMenu;

        public MainMenu(InputTransactionMenu inputTransactionMenu, 
                        DefineInterestRulesMenu defineInterestRulesMenu,
                        PrintStatementMenu printStatementMenu)
        {
            _inputTransactionMenu = inputTransactionMenu;
            _defineInterestRulesMenu = defineInterestRulesMenu;
            _printStatementMenu = printStatementMenu;

            AddMenuOptions();
        }

        public override async Task ProcessUserInputAsync()
        {
            while (true)
            {
                var optionMenuKey = GetUserInput();
                var optionAction = GetOptionAction(optionMenuKey);

                await optionAction();
                ClearConsole();
            }
        }

        private string GetUserInput()
        {
            while (true)
            {
                DisplayMenu();

                var userInput = Console.ReadLine();
                userInput ??= string.Empty;

                if (HasOptionMenuKey(userInput))
                {
                    return userInput.ToUpper();
                }

                ClearConsole();
                PrintLine($"Selected menu option [{userInput}] is not valid.");
            }
        }

        protected override void DisplayMenu()
        {
            PrintLine("\nWelcome to AwesomeGIC Bank! What would you like to do?\n");
            DisplayMenuOptions();
        }

        protected override void AddMenuOptions()
        {
            AddOptionMenuItem("T", "Input transactions", () => _inputTransactionMenu.ProcessUserInputAsync());
            AddOptionMenuItem("I", "Define interest rules", () => _defineInterestRulesMenu.ProcessUserInputAsync());
            AddOptionMenuItem("P", "Print statement", () => _printStatementMenu.ProcessUserInputAsync());
            AddOptionMenuItem("Q", "Quit", () => Task.Run(() => 
            {
                PrintLine("Thank you for banking with AwesomeGIC Bank.");
                PrintLine("Have a nice day!");
                Environment.Exit(0);
            }));
        }
    }
}
