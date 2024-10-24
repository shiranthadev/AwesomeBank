using AwesomeGIC.Bank.Presentation.Dto;

namespace AwesomeGIC.Bank.Presentation.Menus
{
    public abstract class BaseOptionMenu<TActionItem> : BaseMenu
    {
        private readonly Dictionary<string, OptionMenuItem> _optionMenuItems = new(StringComparer.OrdinalIgnoreCase);

        protected abstract void AddMenuOptions();

        protected void AddOptionMenuItem(string optionMenuKey, string optionMenuTitle, Func<Task> optionAction)
        {
            var optionMenuItem = new OptionMenuItem 
            { 
                Key = optionMenuKey, 
                Title = optionMenuTitle, 
                OptionAction = optionAction 
            };

            _optionMenuItems.Add(optionMenuKey, optionMenuItem);
        }

        protected bool HasOptionMenuKey(string optionMenuKey)
        {
            return _optionMenuItems.ContainsKey(optionMenuKey);
        }

        protected Func<Task> GetOptionAction(string optionMenuKey)
        {
            return _optionMenuItems[optionMenuKey].OptionAction;
        }

        protected void DisplayMenuOptions()
        {
            _optionMenuItems.ToList().ForEach(option => PrintLine($"[{option.Key}] {option.Value.Title}"));
        }
    }
}
