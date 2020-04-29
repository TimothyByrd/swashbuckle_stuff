namespace swashbuckle_stuff
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Swashbuckle filter to add "401" and "403" responses for all actions that are decorated with the AuthorizeAttribute.
    /// Added the "403" to the example code at https://github.com/domaindrivendev/Swashbuckle.AspNetCore#operation-filters
    /// </summary>
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        /// <summary>
        /// Applies the filter
        /// </summary>
        /// <param name="operation">The operation (controller API method) to check.</param>
        /// <param name="context">Context information for the operation</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();

            if (authAttributes.Any())
            {
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            }
        }
    }
}
