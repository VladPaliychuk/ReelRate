using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movie.DAL.Data;
using Microsoft.EntityFrameworkCore.Design;
using Movie.BLL.Configurations;
using Movie.BLL.Services;
using Movie.BLL.Services.Interfaces;
using Movie.DAL.Entities;
using Movie.DAL.Infrastructure;
using Movie.DAL.Infrastructure.Interfaces;
using Movie.DAL.Repository;
using Movie.DAL.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddDbContext<MovieContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("MovieDb")));

builder.Services.AddSwaggerGen(
    c => {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie.API", Version = "v1" });
    }
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//services
builder.Services.AddScoped<IActorService, ActorService>();

//repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IActorRepository, ActorRepository>();
builder.Services.AddScoped<IDirectorRepository, DirectorRepository>();
builder.Services.AddScoped<IFilmRepository, FilmRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IFilmActorRepository, FilmActorRepository>();
builder.Services.AddScoped<IFilmDirectorRepository, FilmDirectorRepository>();
builder.Services.AddScoped<IFilmGenreRepository, FilmGenreRepository>();

//context
builder.Services.AddScoped<MovieContext>();

var app = builder.Build();

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

app.MapControllers();

app.Run();