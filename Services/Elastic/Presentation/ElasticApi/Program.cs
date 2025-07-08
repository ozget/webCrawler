using Elastic.Application.Services;
using Elastic.Domain.Repositories;
using Elastic.Infrastructure.Configurations;
using Elastic.Infrastructure.Consumer;
using Elastic.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IElasticRepository, ElasticRepository>();
builder.Services.AddScoped<IElasticService, ElasticService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


// RabbitMQ Configuration
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddHostedService<RabbitMqConsumerService>();
builder.Services.AddElasticSearch("http://localhost:9200");
builder.Services.AddHealthChecks();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseEndpoints(endpoints =>{
//    endpoints.MapHealthChecks("/health");
//});



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
