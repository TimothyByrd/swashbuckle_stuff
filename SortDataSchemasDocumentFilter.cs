namespace swashbuckle_stuff
{
    using System;
    using System.Reflection;

    using Microsoft.OpenApi.Models;

    using Newtonsoft.Json.Serialization;

    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Try to sort the data definitions that appear in the "Schemas" section of the Swagger page.
    /// </summary>
    public sealed class SortDataSchemasDocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Try to sort the data definitions that appear in the "Schemas" section of the Swagger page.
        /// This is an ugly way to do it.
        /// </summary>
        /// <param name="openApiDoc">The swagger document</param>
        /// <param name="context">The filter context</param>
        public void Apply(OpenApiDocument openApiDoc, DocumentFilterContext context)
        {
            if (openApiDoc == null) throw new ArgumentNullException(nameof(openApiDoc));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var schemaList = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, OpenApiSchema>>();
            foreach (var kv in openApiDoc.Components.Schemas)
            {
                schemaList.Add(kv);
            }

            int KvCompare(System.Collections.Generic.KeyValuePair<string, OpenApiSchema> x, System.Collections.Generic.KeyValuePair<string, OpenApiSchema> y)
            {
                return string.Compare(x.Key, y.Key, StringComparison.InvariantCulture);
            }

            schemaList.Sort(KvCompare);

            var newDict = new System.Collections.Generic.Dictionary<string, OpenApiSchema>();

            foreach (var kv in schemaList)
            {
                newDict.Add(kv.Key, kv.Value);
            }

            openApiDoc.Components.Schemas = newDict;
        }
    }
}
