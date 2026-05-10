using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.Common
{

    public interface IResult
    {
        bool IsSuccessful { get; }
        List<string>? Errors { get; }
        public string? ErrorMsg { get; }
        object? GetErrors();
    }

    public class Result : IResult /// dont return anything
    {
        public bool IsSuccessful { get; protected set; }
        public List<string>? Errors { get; protected set; }
        public string? ErrorMsg { get; protected set; }


        public object? GetErrors() => (Errors is not null && Errors.Count > 0) ? Errors : ErrorMsg;


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
                ErrorMsg = errorMsg
            };
        }
        public static Result MultipleFails(List<string> errors)
        {
            return new Result()
            {
                IsSuccessful = false,
                Errors = errors
            };
        }
    }
            
 
    public class Result<T> : Result /// holds important return data
    {
        public T? Value { get; private set; }

        public static Result<T> Success(T value)
        {
           return new Result<T>()
           { IsSuccessful = true,
               Value = value 
           };
        }
        public static new Result<T> Fail(string errorMsg) {

            return new Result<T>()
            { 
                IsSuccessful = false,
                ErrorMsg = errorMsg 
            };
        }
        public static new Result<T> MultipleFails(List<string> errors)
        {
            return new Result<T>()
            {
                IsSuccessful = false,
                Errors = errors
            };
        }
    }
}
