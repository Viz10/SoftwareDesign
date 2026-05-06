
namespace Warehouse.Data
{
    public class Result<T>
    {
        public T? Value { get; set; }
        public string? ErrorMsg { get; set; }
        public bool IsSuccessful { get; set; }


        public Result(bool _success, T? _value, string? _errorMsg)
        {
            IsSuccessful = _success;
            Value = _value;
            ErrorMsg = _errorMsg;
        }


        public static Result<T> Success(T? _value)
        {
            return new Result<T>(true, _value, null);
        }
        public static Result<T> Fail(string? _errorMsg)
        {
            return new Result<T>(false, default, _errorMsg);
        }
    }
}
