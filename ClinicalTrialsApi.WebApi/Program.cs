using ClinicalTrials.Application.ServiceExtensions;
using ClinicalTrials.Infrastructure.Persistance.Context;
using ClinicalTrials.Infrastructure.ServiceExtensions;
using ClinicalTrialsApi.WebApi.ConfigurationExtensions;
using ClinicalTrialsApi.WebApi.Middlewares;
using ClinicalTrialsApi.WebApi.ServiceExtensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Logging
builder.ConfigureLogging();

// Add services to the container.
builder.Services.AddWebApiServices(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.EnableAnnotations();
});

var app = builder.Build();
app.UseExceptionHandler();

//Aleksa
app.UseMiddleware<FileUploadMiddleware>();
//EF Core migrations
using (var serviceScope = app.Services.CreateScope())
{
    var gameDatabase = serviceScope.ServiceProvider.GetRequiredService<ClinicalTrialDbContext>().Database;
    if (gameDatabase.IsRelational())
        gameDatabase.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(new Swashbuckle.AspNetCore.Swagger.SwaggerOptions()
    {
        RouteTemplate = "api/swagger/{documentName}/swagger.json"
    });
    app.UseSwaggerUI((opts) =>
    {
        opts.RoutePrefix = "api/swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseHttpLogging();
app.Run();

public partial class Program { }