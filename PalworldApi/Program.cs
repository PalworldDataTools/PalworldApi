using PalworldApi.Rest.v1;
using PalworldApi.Serialization;
using PalworldApi.Services;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddSingleton<AppJsonSerializerContext>();
builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default); });

builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<RawDataService>(provider => new RawDataService(provider.GetRequiredService<ILogger<RawDataService>>()));

builder.Services.AddV1();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();
}

app.UseExceptionHandler();
app.UseSwaggerUi();

app.UseOpenApi();

app.UseStaticFiles();
app.UseV1();

app.MapGet("/", () => Results.LocalRedirect("~/index.html", true, true));

app.Run();
