
using Crawler.Application.Events;
using Crawler.Application.IServices;
using Crawler.Application.Services;
using Crawler.Domain.Entities;
using Crawler.Infrastructure.Configurations;
using Crawler.Infrastructure.Publishers;
using Crawler.Infrastructure.Worker;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<CrawlerSettings>(builder.Configuration.GetSection("CrawlerSettings"));
builder.Services.AddScoped<ICrawlerService, CrawlerService>();


// AppSettings RabbitMQ ayarlarýný konfigüre et
builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddSingleton<INewsPublisher<NewFetchedEventDto>, RabbitMQPublisher<NewFetchedEventDto>>();


builder.Services.AddHostedService<NewsFetchWorker>();

builder.Services.AddScoped<ICrawlerScopedProcessor, CrawlerScopedProcessor>();



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
