using Microsoft.EntityFrameworkCore;
using ServiceSchedule.Configurations;
using ServiceSchedule.Helpers.Producer;
using ServiceSchedule.Infrastructure;
using ServiceSchedule.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.Configure<KafkaProducerConfig>(builder.Configuration.GetSection(nameof(KafkaProducerConfig)));
builder.Services.Configure<KafkaProducerAccountConfiguration>(builder.Configuration.GetSection(nameof(KafkaProducerAccountConfiguration)));
builder.Services.AddSingleton<IProducerHelper, ProducerHelper>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetSection("ConnectionString");

        options.UseSqlServer(connectionString.Value);
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//migrations
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
dbContext.Database.Migrate();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
