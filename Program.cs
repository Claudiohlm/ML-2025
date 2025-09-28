using Microsoft.ML;
using ML_2025.Services;

var builder = WebApplication.CreateBuilder(args);

// Adiciona suporte a Razor Pages
builder.Services.AddRazorPages();

// Inicializa ML.NET para uso futuro
var mlContext = new MLContext();
builder.Services.AddSingleton(mlContext);

var pastaModelos = Path.Combine(AppContext.BaseDirectory, "MLModels");
if (!Directory.Exists(pastaModelos))
{
    Directory.CreateDirectory(pastaModelos);
}

var modelPath = Path.Combine(pastaModelos, "model.zip");

if (File.Exists(modelPath))
{
    // Carrega o modelo apenas para manter compatibilidade futura
    mlContext.Model.Load(modelPath, out _);
}

// Build do app
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

// Chama o treinamento no startup
ModelBuilder.Treinar(pastaModelos);

app.Run();