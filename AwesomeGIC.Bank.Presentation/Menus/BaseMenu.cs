namespace AwesomeGIC.Bank.Presentation.Menus
{
    public abstract class BaseMenu
    {
        public abstract Task ProcessUserInputAsync();

        protected abstract void DisplayMenu();

        protected static void PrintLine(string message)
        {
            Console.WriteLine(message);
        }

        protected static void ClearConsole()
        {
            Console.Clear();
        }
    }
}
