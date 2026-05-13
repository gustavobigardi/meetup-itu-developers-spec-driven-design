using EventsApi.Data;
using EventsApi.DTOs;
using EventsApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase);

// Usa nosso ErrorResponse customizado para erros de validação (conforme a spec)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var details = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors.Select(e => new ErrorDetail(x.Key, e.ErrorMessage)));

        return new BadRequestObjectResult(
            new ErrorResponse("VALIDATION_ERROR", "Dados inválidos na requisição", details));
    };
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("EventsDb"));

builder.Services.AddScoped<IEventsService, EventsService>();

var app = builder.Build();

// Seed the in-memory database on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DatabaseSeeder.Seed(db);
}

app.MapControllers();

app.Run();
