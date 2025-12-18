using Application.Abstractions.Services;
using Application.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DotnetAssessment.Filters
{
    public class AuditActionFilter : IAsyncActionFilter
    {
        private readonly IAuditService _auditService;
        private readonly ILogger<AuditActionFilter> _logger;
        private readonly ICurrentUser _currentUser;

        public AuditActionFilter(IAuditService auditService, ILogger<AuditActionFilter> logger, ICurrentUser currentUser)
        {
            _auditService = auditService;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            // Only audit successful write operations
            if (executedContext.Exception == null && IsWriteOperation(context.HttpContext.Request.Method))
            {
                try
                {
                    Guid? userId = _currentUser.UserId;

                    var actionName = context.ActionDescriptor.RouteValues["action"] 
                                  ?? context.ActionDescriptor.DisplayName?.Split('.').LastOrDefault() 
                                  ?? "Unknown";
                    var controllerName = context.ActionDescriptor.RouteValues["controller"] ?? "Unknown";
                    
                    var entityName = controllerName.Replace("Controller", "", StringComparison.OrdinalIgnoreCase);
                    
                    string? entityId = null;
                    if (context.RouteData.Values.TryGetValue("id", out var idValue))
                    {
                        entityId = idValue?.ToString();
                    }

                    var action = $"{entityName}.{actionName}";
                    
                    if (string.IsNullOrEmpty(entityId) && executedContext.Result is OkObjectResult objectResult)
                    {
                        var resultValue = objectResult.Value;
                        if (resultValue != null)
                        {
                            object? dataObject = null;
                            var resultType = resultValue.GetType();
                            
                            // Check if it's a Result<T> type by checking if it inherits from Result and has a Data property
                            if (typeof(Result).IsAssignableFrom(resultType) && resultType.IsGenericType)
                            {
                                // Get the Data property from Result<T>
                                var dataProperty = resultType.GetProperty("Data");
                                if (dataProperty != null)
                                {
                                    dataObject = dataProperty.GetValue(resultValue);
                                }
                            }

                            // Try to get Id from the data object
                            if (dataObject != null)
                            {
                                var idProperty = dataObject.GetType().GetProperty("Id");
                                if (idProperty != null)
                                {
                                    var idValueFromResult = idProperty.GetValue(dataObject)?.ToString();
                                    if (!string.IsNullOrEmpty(idValueFromResult))
                                    {
                                        entityId = idValueFromResult;
                                    }
                                }
                            }
                        }
                    }
                    
                    await _auditService.RecordAsync(
                        action: action,
                        entityName: entityName,
                        entityId: entityId ?? "N/A",
                        userId: userId != Guid.Empty ? userId : null,
                        ct: context.HttpContext.RequestAborted);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to audit action: {Action}", context.ActionDescriptor.DisplayName);
                }
            }
        }

        private static bool IsWriteOperation(string method)
        {
            return method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("PUT", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("PATCH", StringComparison.OrdinalIgnoreCase) ||
                   method.Equals("DELETE", StringComparison.OrdinalIgnoreCase);
        }
    }
}

