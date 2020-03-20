using System;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace swashbuckle_stuff
{
    /// <summary>
    /// Disable additional properties for everything.
    /// Otherwise the NSwag SDK code generator adds an AdditionalProperties dictionary to every class.
    /// </summary>
    public sealed class NoAdditionalPropertiesSchemaFilter : ISchemaFilter
    {
        /// <summary>
        /// Sets AdditionalPropertiesAllowed  to false for all models.
        /// </summary>
        /// <param name="model">The schema model</param>
        /// <param name="context">The schema filter context</param>
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));

            model.AdditionalPropertiesAllowed = false;
        }
    }
}