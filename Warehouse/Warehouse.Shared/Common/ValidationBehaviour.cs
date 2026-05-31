using FluentValidation;
using MediatR;

namespace Warehouse.Shared.Common
{

    public class ValidationBehaviour<TRequest, TResponse>
        (IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : IResult
    {

        /// TRequest = command send to handler
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators; /// set of validators for object


        public async Task<TResponse> Handle(TRequest request,RequestHandlerDelegate<TResponse> next,CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return await next(cancellationToken); /// no validators for this command , return handler result

            var context = new ValidationContext<TRequest>(request);

            var failures = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))))
                .SelectMany(r => r.Errors)
                .Where(e => e != null)
                .ToList();

            if (failures.Count == 0) return await next(cancellationToken); /// no error

            var fail =  /// reflection to pass Result back
                     typeof(TResponse).GetMethod("MultipleFails", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)!
                    .Invoke(null, [failures])!;

            return (TResponse)fail; /// Result or Result<T> with multiple errors
        }
    }
}
