var builder = WebApplication.CreateBuilder(args);

// Adicione suporte a CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Permite qualquer origem
              .AllowAnyHeader() // Permite todos os cabeçalhos
              .AllowAnyMethod(); // Permite todos os métodos (GET, POST, etc.)
    });
});

// Adicione os serviços ao container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita CORS no pipeline
app.UseCors();

// HTTPS redirection (opcional, pode ser desativado se só usar HTTP)
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
