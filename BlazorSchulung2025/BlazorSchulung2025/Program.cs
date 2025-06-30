using System.Text.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;
using BlazorSchulung2025;
using BlazorSchulung2025.Components;
using BlazorSchulung2025.Models;
using BlazorSchulung2025.Validators;
using FluentValidation;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Sensitive;
using Sdk = OpenTelemetry.Sdk;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((
        context,
        loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithSensitiveDataMasking(options => options.ExcludeProperties.AddRange(["Password", "Email"]))
        .Destructure.ByTransforming<Exception>(ex => new
            { ex.Message, ex.StackTrace, ex.Source, ex.TargetSite, ex.InnerException })
);

var logger = Log.Logger;
logger.Information("Starting up");

var otlpExporterOptions = new OtlpExporterOptions();
otlpExporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
otlpExporterOptions.Endpoint = new Uri("http://abz-azure-03.abzi.eu:6452/ingest/otlp/v1/traces");
otlpExporterOptions.Headers = "X-Seq-ApiKey=KrPpDyB8oDxFOc4Smrrd";

builder.Logging.AddOpenTelemetry(options =>
{
    options
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("BlazorSchulung2025.BlazorApp"))
        .AddConsoleExporter()
        .AddOtlpExporter(otlpOptions =>
        {
            otlpOptions.Protocol = otlpExporterOptions.Protocol;
            otlpOptions.Endpoint = otlpExporterOptions.Endpoint;
            otlpOptions.Headers = otlpExporterOptions.Headers;
        });
});

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("BlazorSchulung2025"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSource("BlazorSchulung2025.BlazorApp");
        tracing
            .AddOtlpExporter(options =>
            {
                options.Protocol = otlpExporterOptions.Protocol;
                options.Endpoint = otlpExporterOptions.Endpoint;
                options.Headers = otlpExporterOptions.Headers;
            });
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddProcessInstrumentation()
            .AddRuntimeInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEventCountersInstrumentation(options =>
            {
                options.AddEventSources("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Hosting.Diagnostics");
                options.AddEventSources("System.Net.Http");
            });
        metrics
            .AddOtlpExporter(options =>
            {
                options.Protocol = otlpExporterOptions.Protocol;
                options.Endpoint = otlpExporterOptions.Endpoint;
                options.Headers = otlpExporterOptions.Headers;
            });
    });

using var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MySample"))
    .AddSource("Sample.DistributedTracing")
    .AddConsoleExporter()
    .Build();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Registrieren des LifecycleLogService als Singleton
builder.Services.AddSingleton<LifecycleLogService>();

// Registrieren des ApplicationState als Scoped Service
builder.Services.AddScoped<ApplicationState>();

// Registrieren des UserInfoValidators als Service
builder.Services.AddTransient<IValidator<UserInfo>, UserInfoValidator>();

builder.Services.AddBlazoredLocalStorage(config =>
{
    config.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    config.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    config.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
    config.JsonSerializerOptions.WriteIndented = false;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();