using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace Warehouse.Shared.Common
{
    public interface IResult
    {
         bool IsSuccessful { get; }
         Dictionary<string, List<string>>? Errors { get; }
    }

    public class Result : IResult /// dont return anything
    {
        [JsonPropertyName("isSuccessful")]
        [JsonInclude]
        public bool IsSuccessful { get; protected set; }
        
        [JsonPropertyName("errors")]
        [JsonInclude]
        public Dictionary<string, List<string>>? Errors { get; protected set; }



        public static Result Success()
        {
            return new Result()
            {
                IsSuccessful = true
            };
        }
        public static Result Fail(string errorMsg)
        {
            return new Result()
            {
                IsSuccessful = false,
                Errors = new Dictionary<string, List<string>> { ["Error"] = [errorMsg] }
            };
        }
        public static Result MultipleFails(List<ValidationFailure> failures)
        {
            var fieldErrors = failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToList()
            );

            return new Result { IsSuccessful = false, Errors = fieldErrors };
        }
    }
            
 
    public class Result<T> : Result /// holds important data
    {
        [JsonPropertyName("value")]
        [JsonInclude]
        public T? Value { get; private set; } /// deserialization prop setting works via [JsonInclude]


        public static Result<T> Success(T value)
        {
           return new Result<T>()
           { 
               IsSuccessful = true,
               Value = value 
           };
        }
        public new static Result<T> Fail(string errorMsg) {

            return new Result<T>()
            { 
                IsSuccessful = false,
                Errors = new Dictionary<string, List<string>> { ["Error"] = [errorMsg] }
            };
        }
        public new static Result<T> MultipleFails(List<ValidationFailure> failures)
        {
            var fieldErrors = failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToList()
            );

            return new Result<T> { IsSuccessful = false, Errors = fieldErrors };
        }
    }
}
