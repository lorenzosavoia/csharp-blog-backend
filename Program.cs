//per il context aggiungiamo:
using Microsoft.EntityFrameworkCore;
using csharp_blog_backend.Models;

var builder = WebApplication.CreateBuilder(args);

// MODIFICA AGGIUNTA
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddControllers();
//per il context aggiungiamo
builder.Services.AddDbContext<BlogContext>(opt =>
    opt.UseInMemoryDatabase("posts"));
//end per il context aggiungiamo:



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//MODIFICA AGGIUNTA
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
