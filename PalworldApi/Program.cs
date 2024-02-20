using Microsoft.AspNetCore.Mvc;
using PalworldApi;
using PalworldApi.Rest.v1;
using PalworldApi.Serialization;
using PalworldApi.Services;
using Serilog;

Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateBootstrapLogger();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddProblemDetails();
    builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
    builder.Services.AddResponseCaching();

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

    builder.Services.AddSingleton<AppJsonSerializerContext>();
    builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default); });

    builder.Services.AddSingleton<RawDataService>(provider => new RawDataService(provider.GetRequiredService<ILogger<RawDataService>>()));

    builder.Services.AddMvc();
    builder.Services.AddControllers(
        opt =>
        {
            opt.CacheProfiles.Add(
                Constants.ResponseCacheDefaultProfile,
                new CacheProfile { Duration = (int)TimeSpan.FromMinutes(1).TotalSeconds, Location = ResponseCacheLocation.Any }
            );
            opt.CacheProfiles.Add(
                Constants.ResponseCacheLongTermProfile,
                new CacheProfile { Duration = (int)TimeSpan.FromDays(1).TotalSeconds, Location = ResponseCacheLocation.Any }
            );
        }
    );
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddV1();

    builder.Logging.ClearProviders();
    builder.Logging.AddAzureWebAppDiagnostics();

    builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services), writeToProviders: true);

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

    app.UseCors();

    app.UseResponseCaching();

    app.MapDefaultControllerRoute();

    app.Run();
}
catch (Exception exn)
{
    Log.Fatal(exn, "Unhandled exception.");
}
finally
{
    await Log.CloseAndFlushAsync();
}
