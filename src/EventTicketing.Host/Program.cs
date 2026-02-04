using EventTicketing.BL.Interfaces;
using EventTicketing.BL.Services;
using EventTicketing.DAL.Interfaces;
using EventTicketing.DAL.Mongo;
using EventTicketing.DAL.Repositories;
using EventTicketing.Host.Options;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Options
builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection("Mongo"));
builder.Services.Configure<TicketingOptions>(builder.Configuration.GetSection("Ticketing"));

// Mongo
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongo = sp.GetRequiredService<IOptionsMonitor<MongoOptions>>().CurrentValue;
    return new MongoClient(mongo.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongo = sp.GetRequiredService<IOptionsMonitor<MongoOptions>>().CurrentValue;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongo.DatabaseName);
});

builder.Services.AddSingleton<MongoContext>();

// DAL
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// BL
builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<ITicketService, TicketService>();

// HealthChecks (Mongo)
var mongoConn = builder.Configuration["Mongo:ConnectionString"];
builder.Services.AddHealthChecks()
    .AddMongoDb(mongoConn!, name: "mongo");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // OFF

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                error = e.Value.Exception?.Message
            })
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.MapControllers();
app.Run();
