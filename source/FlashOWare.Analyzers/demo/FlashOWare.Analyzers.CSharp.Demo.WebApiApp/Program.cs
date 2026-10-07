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

app.MapGet("/", static Ok<Payload> (HttpContext context) =>
	{
		Payload payload = new("Hello, World!");
		return TypedResults.Ok(payload);
	})
	.WithName("Get");

app.Run();

internal sealed record class Payload(string Text);

[JsonSerializable(typeof(Payload))]
internal sealed partial class AppJsonSerializerContext : JsonSerializerContext;
