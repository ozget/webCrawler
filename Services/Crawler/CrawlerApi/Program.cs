
using Crawler.Application.IServices;
using Crawler.Application.Services;
using Crawler.Domain.Entities;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<CrawlerSettings>(builder.Configuration.GetSection("CrawlerSettings"));


//AngleSharp siteden verileri okuyabilmek icin
//builder.Services.AddSingleton<IBrowsingContext>(_ =>
//    BrowsingContext.New(Configuration.Default.WithDefaultLoader()));

builder.Services.AddScoped<ICrawlerService, CrawlerService>();




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
