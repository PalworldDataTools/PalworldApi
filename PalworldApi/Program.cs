using PalworldApi.Rest.v1;
using PalworldApi.Serialization;
using PalworldApi.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AppJsonSerializerContext>();
builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default); });

builder.Services.AddSingleton<RawDataService>(provider => new RawDataService(provider.GetRequiredService<ILogger<RawDataService>>()));

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddV1();

builder.Services.AddProblemDetails();

builder.Logging.AddAzureWebAppDiagnostics();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseOpenApi();
app.UseSwaggerUi();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();
