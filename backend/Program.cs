using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Congratz.backend.Mappings;
using Congratz.backend.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       builder.Configuration["POSTGRES_CONNECTION"];

builder.Services.AddDbContext<BirthdayContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(BirthdayMappingProfile).Assembly);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();