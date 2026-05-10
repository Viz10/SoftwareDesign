using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.Common
{

    public interface IResult
    {
        bool IsSuccessful { get; }
        public Dictionary<string, List<string>>? FieldErrors { get; } /// API related
        public string? ErrorMsg { get; } /// system related
        object? GetErrors();
    }

    public class Result : IResult /// dont return anything
    {

        public bool IsSuccessful { get; protected set; }
        public Dictionary<string, List<string>>? FieldErrors { get; protected set; }
        public string? ErrorMsg { get; protected set; }


        public object? GetErrors() => (FieldErrors is not null && FieldErrors.Count > 0) ? FieldErrors : ErrorMsg;


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
        public static Result MultipleFails(List<ValidationFailure> failures)
        {
            var fieldErrors = failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToList()
            );

            return new Result { IsSuccessful = false, FieldErrors = fieldErrors };
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
        public new static Result<T> MultipleFails(List<ValidationFailure> failures)
        {
            var fieldErrors = failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(f => f.ErrorMessage).ToList()
            );

            return new Result<T> { IsSuccessful = false, FieldErrors = fieldErrors };
        }
    }
}
