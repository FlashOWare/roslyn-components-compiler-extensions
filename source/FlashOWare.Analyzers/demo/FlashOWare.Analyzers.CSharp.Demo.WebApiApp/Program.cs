using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Json;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(static (JsonOptions options) =>
{
	options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddOpenApi();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.MapGet("/", static Ok<RootPayload> (HttpContext context) =>
	{
		RootPayload payload = new("Hello, World!");
		return TypedResults.Ok(payload);
	})
	.WithName("Get");

app.MapGet("/assembly", static Ok<AssemblyPayload> (HttpContext context) =>
{
	Assembly assembly = typeof(Program).Assembly;
	AssemblyName name = assembly.GetName();
	AssemblyPayload payload = new(name.Name, name.Version, name.CultureName);
	return TypedResults.Ok(payload);
})
	.WithName("GetAssembly");
app.Run();

internal sealed record class RootPayload(string Text);
internal sealed record class AssemblyPayload(string? Name, Version? Version, string? Culture);

[JsonSerializable(typeof(RootPayload))]
[JsonSerializable(typeof(AssemblyPayload))]
internal sealed partial class AppJsonSerializerContext : JsonSerializerContext;
