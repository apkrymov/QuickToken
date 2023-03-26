using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuickToken.Core.Shared.Services.Exceptions;
using QuickToken.Facade.Contracts;

namespace QuickToken.Facade.Filters;

public class ExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<ExceptionFilter> _logger;
    private readonly IWebHostEnvironment _environment;

    public ExceptionFilter(ILogger<ExceptionFilter> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    public override void OnException(ExceptionContext context)
    {
        if (context.ExceptionHandled)
        {
            return;
        }

        switch (context.Exception)
        {
            case TransactionFailedException transactionFailedException:
                _logger.LogWarning(transactionFailedException, "An error occured during transaction execution");
                EnrichTransactionFailedExceptionContext(context, transactionFailedException);
                break;
            
            case TransactionNotFoundException transactionNotFoundException:
                EnrichTransactionNotFoundExceptionContext(context, transactionNotFoundException);
                break;
            
            case { } exception:
                _logger.LogCritical(exception, "An unhandled error occured during request");
                EnrichUnhandledExceptionContext(context, exception);
                break;
        }
    }

    private void EnrichTransactionFailedExceptionContext(in ExceptionContext context, TransactionFailedException exception)
    {
        var errorModel = new ErrorResponse
        {
            Message = exception.Message
        };

        if (_environment.IsDevelopment())
        {
            errorModel.Details = $"Error: {exception.Response.Message}. Stacktrace: {exception.Response.Stacktrace}";
        }

        context.Result = new ObjectResult(errorModel)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;
    }
    
    private void EnrichTransactionNotFoundExceptionContext(in ExceptionContext context, TransactionNotFoundException exception)
    {
        var errorModel = new ErrorResponse
        {
            Message = exception.Message
        };

        context.Result = new ObjectResult(errorModel)
        {
            StatusCode = StatusCodes.Status404NotFound
        };
        context.ExceptionHandled = true;
    }

    private void EnrichUnhandledExceptionContext(in ExceptionContext context, Exception exception)
    {
        var errorModel = new ErrorResponse
        {
            Message = "An unexpected error occured"
        };

        context.Result = new ObjectResult(errorModel)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;
    }
}