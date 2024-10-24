namespace AwesomeGIC.Bank.Presentation.Dto
{
    internal record OptionMenuItem
    {
        public required string Key { get; init; }

        public required string Title { get; init; }

        public required Func<Task> OptionAction { get; init; }
    }
}
