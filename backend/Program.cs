using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Congratz.backend.Mappings;
using Congratz.backend.Context;
using Congratz.backend.Seeder;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       builder.Configuration["POSTGRES_CONNECTION"];

builder.Services.AddDbContext<BirthdayContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(BirthdayMappingProfile).Assembly);

builder.WebHost.UseUrls("http://0.0.0.0:80");

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BirthdayContext>();

    await DbInitializer.InitializeAsync(context);

}

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
