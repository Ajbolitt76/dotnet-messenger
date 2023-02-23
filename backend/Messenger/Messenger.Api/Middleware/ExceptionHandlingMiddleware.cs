using System.Net;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Messenger.Core.Exceptions;
using Messenger.Infrastructure.Exceptions;

namespace Messenger.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerFactory _loggerFactory;
    private readonly IWebHostEnvironment _hostingEnvironment;

    /// <summary>
    /// Обработчик ошибок API
    /// </summary>
    /// <param name="next">Делегат следующего обработчика в пайплайне ASP.NET Core</param>
    /// <param name="loggerFactory">Фабрика логгеров</param>
    /// <param name="hostingEnvironment">Хост</param>
    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILoggerFactory loggerFactory,
        IWebHostEnvironment hostingEnvironment)
    {
        _next = next;
        _loggerFactory = loggerFactory;
        _hostingEnvironment = hostingEnvironment;
    }

    /// <summary>
    /// Действия по обработке запроса ASP.NET
    /// </summary>
    /// <param name="context">Контекст запроса ASP.NET</param>
    /// <returns>Задача на обработку запроса ASP.NET</returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (OutOfMemoryException ex)
        {
        }
        catch (UnauthorizedAccessException ex)
        {
            await HandleUnauthorizedAccessExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Создать логгер для контроллера, исполнявшего запрос
    /// </summary>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Логгер</returns>
    private ILogger GetLogger(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<MethodInfo>();
        var controllerType = controllerActionDescriptor?.DeclaringType;
        return controllerType != null
            ? _loggerFactory.CreateLogger(controllerType)
            : _loggerFactory.CreateLogger<ExceptionHandlingMiddleware>();
    }

    // Переделать, убрать доменные исключения из обработки
    private async Task LogAndReturnAsync(
        HttpContext context,
        Exception exception,
        string errorText,
        HttpStatusCode responseCode,
        LogLevel logLevel,
        Dictionary<string, object>? details = null)
    {
        details ??= new Dictionary<string, object>();
        details.Add("traceId", context.TraceIdentifier);
        GetLogger(context).Log(logLevel, exception, errorText, details);
        
        if (exception is DomainException ex)
        {
            await WriteDomainError(context, ex);
            return;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)responseCode;

        var response = new ProblemDetails
        {
            Title = errorText,
            Instance = null,
            Status = (int)responseCode,
            Type = null,
            Detail = errorText
        };
        
        if(!_hostingEnvironment.IsProduction())
            response.Extensions.Add("trace", exception.StackTrace);

        foreach (var detail in details)
            response.Extensions.Add(detail.Key, detail.Value);

        var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>();
        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response, jsonOptions.Value.JsonSerializerOptions));
    }


    private Task WriteDomainError(HttpContext context, DomainException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)exception.StatusCode;

        var problem = DomainExceptionToProblemDetails.ToProblemDetails(exception);
        
        if(!_hostingEnvironment.IsProduction())
            problem.Extensions.Add("trace", exception.StackTrace);
        problem.Extensions.Add("traceId", context.TraceIdentifier);

        var jsonOptions = context.RequestServices.GetRequiredService<IOptions<JsonOptions>>();
        
        return context.Response.WriteAsync(
            JsonSerializer.Serialize(
                problem,
                jsonOptions.Value.JsonSerializerOptions));
    }


    private async Task HandleUnauthorizedAccessExceptionAsync(HttpContext context,
        UnauthorizedAccessException exception)
    {
        var errorText = "FORBIDDEN";
        var logLevel = LogLevel.Warning;
        var responseCode = HttpStatusCode.Forbidden;
        await LogAndReturnAsync(context, exception, errorText, responseCode, logLevel);
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorText = exception.Message;
        var logLevel = LogLevel.Error;
        var responseCode = HttpStatusCode.InternalServerError;
        await LogAndReturnAsync(context, exception, errorText, responseCode, logLevel);
    }
}