var builder = WebApplication.CreateBuilder(args);

// Adicione suporte a CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Permite qualquer origem
              .AllowAnyHeader() // Permite todos os cabe�alhos
              .AllowAnyMethod(); // Permite todos os m�todos (GET, POST, etc.)
    });
});

// Adicione os servi�os ao container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline de requisi��o HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita CORS no pipeline
app.UseCors();

// HTTPS redirection (opcional, pode ser desativado se s� usar HTTP)
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
