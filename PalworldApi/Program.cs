using System.Text.Json.Serialization;
using PalworldApi.Rest.OpenApi.PalworldVersion;
using PalworldApi.Rest.v1;
using PalworldApi.Services;
using Serilog;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;

ReloadableLogger logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateBootstrapLogger();
SerilogLoggerFactory loggerFactory = new(logger);


Log.Logger = logger;

try
{
    RawDataService rawDataService = new(loggerFactory.CreateLogger<RawDataService>());
    LocalizationService localizationService = new(rawDataService);

    string[] versions = rawDataService.GetVersions().ToArray();
    string[] languages = (await localizationService.GetLanguages()).ToArray();

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // makes enums in responses strings instead of numbers
    builder.Services.ConfigureHttpJsonOptions(opt => opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    builder.Services.AddProblemDetails();
    builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

    builder.Services.AddSingleton(rawDataService);
    builder.Services.AddSingleton(localizationService);
    builder.Services.AddSingleton<PalIconsService>();

    builder.Services.AddMvc()
        // makes NSwag generate schemas with string values for enum types
        .AddJsonOptions(opt => opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

    builder.Services.AddRequestLocalization(
        opt =>
        {
            opt.AddSupportedCultures(languages);
            opt.AddSupportedUICultures(languages);
            opt.SetDefaultCulture(LocalizationService.DefaultLanguage);
            opt.ApplyCurrentCultureToResponseHeaders = true;
        }
    );
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddV1(opt => opt.ConfigureLanguages(languages, LocalizationService.DefaultLanguage).ConfigureVersions(versions, RawDataService.DefaultVersion));

    builder.Logging.ClearProviders();
    builder.Logging.AddAzureWebAppDiagnostics();

    builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(services), writeToProviders: true);

    WebApplication app = builder.Build();

    app.UseRequestLocalization();
    app.UseMiddleware<PalworldVersionMiddleware>();

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
