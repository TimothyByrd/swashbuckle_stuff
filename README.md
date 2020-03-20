# swashbuckle_stuff
Some sample filters for use with Swashbuckle

Too trivial to make Nuget packages - just copy the source of the filters you want into your project.

I'm using Swashbuckle.AspNetCore.Newtonsoft, so if you are not, make appropriate changes.

# FixIssuesDocumentFilter.cs

Document filter to:

1. Make required API arguments non-nullable - should be fixed in SwashBuckle 5.2, but meanwhile...

This causes an NSwag generated C# client to make null checks against the parameters.

2. Make API bodies required.

I didn't want to have to annotate all body parameter with [BindRequired] attributes.
I want this to cause NSwag to make a null check against the body parameter, but not there yet.

https://github.com/RicoSuter/NSwag/issues/2736

# FixNullableEnumsSchemaFilter.cs

Makes nullable enums nullable in swagger.
Unfortunately, this does not affect the generated JSON in SwashBuckle 5.1.0.
So this filter is probably not useful.

See https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/861

# NoAdditionalPropertiesSchemaFilter.cs

Sets AdditionalPropertiesAllowed  to false for all models.
Otherwise the NSwag SDK code generator adds an AdditionalProperties dictionary to every class.

# RequireValueTypePropertiesSchemaFilter.cs

Original code from https://stackoverflow.com/questions/46576234/swashbuckle-make-non-nullable-properties-required
Thanks crimbo!
Updated for Swashbuckle 5.1.0

Makes all value-type properties "Required" in the schema docs, which is appropriate since they cannot be null.
I didn't want to have to annotate everything with [Required] attributes.
