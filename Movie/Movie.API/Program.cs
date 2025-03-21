using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movie.DAL.Data;
using Microsoft.EntityFrameworkCore.Design;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<MovieContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MovieDb")));

builder.Services.AddSwaggerGen(
    c => {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie.API", Version = "v1" });
    }
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(
        c => {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie.API v1");
        }
    );
}

app.Run();