using FastEndpoints.Swagger;
using PriceWatcher.Server.Data;
using PriceWatcher.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterDbContext<ApplicationDbContext>("DefaultConnection");
builder.Services.RegisterFastEndpoints();
builder.Services.RegisterSwaggerDocument();
builder.Services.RegisterItemWatcher();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultExceptionHandler();

app.UseRegisteredEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.Run();
