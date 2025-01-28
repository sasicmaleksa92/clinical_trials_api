namespace ClinicalTrials.Application.Common.ResultPattern
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public T Value { get; }

        private Result(T value, bool isSuccess, string error)
        {
            Value = value;
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result<T> Success(T value) => new(value, true, null);
        public static Result<T> Failure(string error)
        {
            return new(default, false, error);
        }

        public static implicit operator bool(Result<T> result) => result.IsSuccess;
    }
}
