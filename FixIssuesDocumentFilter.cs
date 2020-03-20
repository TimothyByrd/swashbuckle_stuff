using System;
using System.Reflection;

using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace swashbuckle_stuff
{
    /// <summary>
    /// Document filter to:
    /// - Make required API arguments non-nullable - should be fixed in SwashBuckle 5.2, but meanwhile...
    /// - Make API bodies required.
    /// </summary>
    public sealed class FixIssuesDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Make required API arguments non-nullable - should be fixed in SwashBuckle 5.2, but meanwhile...
        /// Make API bodies required.
        /// </summary>
        /// <param name="openApiDoc">The swagger document</param>
        /// <param name="context">The filter context</param>
        public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
        {
            if (openApiDoc == null) throw new ArgumentNullException(nameof(openApiDoc));
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var pathKv in openApiDoc.Paths)
            {
                foreach (var operationKv in pathKv.Value.Operations)
                {
                    var operation = operationKv.Value;

                    // This block makes parameters to API methods non-nullable
                    // This causes an NSwag generated C# client to make null checks against the parameters.
                    //
                    var parameters = operation.Parameters;
                    foreach (var parameter in operationKv.Value.Parameters)
                    {
                        if (parameter.Required && parameter.Schema != null && parameter.Schema.Nullable)
                        {
                            parameter.Schema.Nullable = false;
                        }
                    }

                    // This block makes the API body required.
                    // I want this to cause NSwag to make a null check, but not there yet.
                    //
                    if (operation.RequestBody != null)
                    {
                        operation.RequestBody.Required = true;
                    }
                }
            }
        }
    }
}
