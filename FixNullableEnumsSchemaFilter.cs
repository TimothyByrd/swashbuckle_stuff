using System;
using System.Reflection;

using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace swashbuckle_stuff
{
    //
    // WARNING: This filter seemz to not affect the generated OpenAPI 3.0 swagger document.
    //
    // See https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/861
    //
    // I wanted this code:
    //
    //     public class MyClass1
    //     {
    //         public MyEnum RequiredEnum { get; set; }
    //         public MyEnum? OptionalEnum { get; set; }
    //     }
    //
    // To generate this swagger:
    //
    //     "MyClass1": {
    //         "type": "object",
    //         "properties": {
    //             "requiredEnum": {
    //                 "$ref": "#/components/schemas/MyEnum"
    //             },
    //             "optionalEnum": {
    //                 "$ref": "#/components/schemas/MyEnum",
    //                 "nullable": true
    //             }
    //         }
    //     }
    //
    // But it still generates this:
    //
    //     "MyClass1": {
    //         "type": "object",
    //         "properties": {
    //             "requiredEnum": {
    //                 "$ref": "#/components/schemas/MyEnum"
    //             },
    //             "optionalEnum": {
    //                 "$ref": "#/components/schemas/MyEnum"
    //             }
    //         }
    //     }
    //

    /// <summary>
    /// Makes nullable enums nullable in swagger.
    /// </summary>
    public sealed class FixNullableEnumsSchemaFilter : ISchemaFilter
    {
        private readonly CamelCasePropertyNamesContractResolver _camelCaseContractResolver;

        /// <summary>
        /// Initializes a new <see cref="FixNullableEnumsSchemaFilter"/>.
        /// </summary>
        /// <param name="camelCasePropertyNames">If <c>true</c>, property names are expected to be camel-cased in the JSON schema.</param>
        /// <remarks>
        /// When adding this in AddSwaggerGen, remember to pass in a boolean about whether to use camel case or not.
        /// </remarks>
        public FixNullableEnumsSchemaFilter(bool camelCasePropertyNames)
        {
            _camelCaseContractResolver = camelCasePropertyNames ? new CamelCasePropertyNamesContractResolver() : null;
        }

        /// <summary>
        /// Returns the JSON property name for <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property name</param>
        /// <returns></returns>
        private string PropertyName(PropertyInfo property)
        {
            return _camelCaseContractResolver?.GetResolvedPropertyName(property.Name) ?? property.Name;
        }

        /// <summary>
        /// Sets nullable enum properties to be nullable.
        /// </summary>
        /// <param name="model">The schema model</param>
        /// <param name="context">The schema filter context</param>
        public void Apply(OpenApiSchema model, SchemaFilterContext context)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (model.Properties == null) return;

            foreach (var property in context.Type.GetProperties())
            {
                if (property.PropertyType.Name.StartsWith("Nullable`", StringComparison.OrdinalIgnoreCase) && property.PropertyType.GenericTypeArguments[0].IsEnum)
                {
                    string schemaPropertyName = PropertyName(property); // camel case if necessary
                    if (model.Properties.TryGetValue(schemaPropertyName, out var modelProperty))
                    {
                        modelProperty.Nullable = true;
                    }
                }
            }
        }
    }
}
