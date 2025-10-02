using JLStore.Domain.Repositories;
using JLStore.Domain.Services;
using JLStore.Infrastructure.Data;
using JLStore.Infrastructure.Repositories;
using JLStore.Mapping;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var conn = builder.Configuration.GetConnectionString("Default")
           ?? "Data Source=./Data/jlstore.db";

builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlite(conn));

// DI
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// AutoMapper (v15+)
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<CustomerProfile>();
});


// API + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "JLStore API v1");
        o.RoutePrefix = string.Empty; // UI su "/"
    });
}

app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<DataContext>();
    await DataSeed.EnsureSeedAsync(ctx);
}

await app.RunAsync();
