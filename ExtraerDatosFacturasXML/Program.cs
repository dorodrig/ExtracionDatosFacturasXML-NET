using ExtraerDatosFacturasXML.Models;
using ExtraerDatosFacturasXML.Services;
using Microsoft.AspNetCore.Http.Features;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddRazorPages();

// Configurar límites de upload
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
});

// Registrar configuración de VisualVault
builder.Services.Configure<VisualVaultSettings>(
    builder.Configuration.GetSection("VisualVault")
);

// Registrar HttpClient con Polly para reintentos
builder.Services.AddHttpClient<VisualVaultService>()
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt => 
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
        )
    );

// Registrar servicios
builder.Services.AddScoped<XmlParserService>();
builder.Services.AddScoped<FacturaProcessingService>();

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Configurar pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UsePathBase("/extracion");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
