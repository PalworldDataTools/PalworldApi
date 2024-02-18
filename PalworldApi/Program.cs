using CUE4Parse.UE4.Versions;
using PalworldApi.Configuration;
using PalworldApi.Serialization;
using PalworldApi.Services;
using PalworldApi.v1;
using PalworldDataExtractor;

WebApplicationBuilder builder = WebApplication.CreateSlimBuilder(args);

DataExtractionConfiguration config = GetConfiguration(builder.Configuration);
builder.Services.AddSingleton(config);

builder.Services.AddSingleton<AppJsonSerializerContext>();
builder.Services.ConfigureHttpJsonOptions(options => { options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default); });

builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<RawDataService>(
    provider => new RawDataService(provider.GetRequiredService<DataExtractionConfiguration>(), provider.GetRequiredService<ILogger<RawDataService>>())
);

builder.Services.AddV1();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();

    app.UseSwaggerUi();
}

app.UseExceptionHandler();

app.UseOpenApi();

RawDataService rawDataService = app.Services.GetRequiredService<RawDataService>();
await rawDataService.Initialize();

app.UseV1();

app.Run();

return;

DataExtractionConfiguration GetConfiguration(IConfiguration configuration)
{
    return new DataExtractionConfiguration
    {
        PalworldPakDirectory = configuration.GetValue<string>("Extraction:PalworldPakDirectory") ?? "",
        ExtractorConfiguration = new DataExtractorConfiguration
        {
            MappingsFilePath = configuration.GetValue<string>("Extraction:MappingsFilePath") ?? "",
            PakFileName = configuration.GetValue<string>("Extraction:PakFileName") ?? "",
            UnrealEngineVersion = new VersionContainer(EGame.GAME_UE5_1)
        }
    };
}
