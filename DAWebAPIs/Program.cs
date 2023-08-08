using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using DAWebAPIs.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Cosmos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connStr = builder.Configuration.GetSection("CosmosDb").GetValue<string>("connStr");
string dbName = builder.Configuration.GetSection("CosmosDb").GetValue<string>("dbName");
string apiKey = builder.Configuration.GetSection("X-SERVICE-KEY").Value;

var cosmosClient = new CosmosClient(connStr);

builder.Services.AddSingleton(cosmosClient);

List<KeyValuePair<string, string>> keys = new List<KeyValuePair<string, string>>();
keys.Add(new KeyValuePair<string, string>("DBName", dbName));
keys.Add(new KeyValuePair<string, string>("APIKey", apiKey));
builder.Services.AddSingleton(keys);

builder.Services.AddDbContext<AgencyDBContext>(option => option.UseCosmos(connStr, dbName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
