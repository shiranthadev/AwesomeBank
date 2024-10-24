namespace AwesomeGIC.Bank.Dto.Common
{
    public class Result
    {
        private Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        private Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public bool IsSuccess { get; }

        public string Message { get; } = string.Empty;

        public static Result Success() => new(true);

        public static Result Failure(string message) => new(false, message);

    }
}
