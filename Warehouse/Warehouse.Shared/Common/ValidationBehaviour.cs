using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.Common
{
    /// TRequest = command send to handler
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
        {

            if (!_validators.Any()) return await next(); /// no validators for this command , return handler result

            var context = new ValidationContext<TRequest>(request);

            var failures = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))))
                .SelectMany(r => r.Errors)
                .Where(e => e != null)
                .ToList();

            if (!failures.Any()) return await next(); /// ok data

            var fail = typeof(TResponse)
                .GetMethod("MultipleFails", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)!
                .Invoke(null, new object[] { failures })!;

            return (TResponse)fail; /// Result or Result<T> with multiple errors
        }
    }
}
