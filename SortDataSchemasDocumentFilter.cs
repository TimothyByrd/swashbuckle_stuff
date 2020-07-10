namespace swashbuckle_stuff
{
    using System;
    using System.Reflection;

    using Microsoft.OpenApi.Models;

    using Newtonsoft.Json.Serialization;

    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Sort the data definitions that appear in the "Schemas" section of the Swagger page.
    /// </summary>
    public sealed class SortDataSchemasDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Sort the data definitions that appear in the "Schemas" section of the Swagger page.
        /// </summary>
        /// <param name="openApiDoc">The swagger document</param>
        /// <param name="context">The filter context</param>
        public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
        {
            if (openApiDoc == null) throw new ArgumentNullException(nameof(openApiDoc));
            if (context == null) throw new ArgumentNullException(nameof(context));

            openApiDoc.Components.Schemas = new System.Collections.Generic.SortedDictionary<string, OpenApiSchema>(openApiDoc.Components.Schemas);
        }
    }
}
