using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedz.AspNetCore.Fop.MinimalApi.Extensions;
using WebApiExample.Entities;
using WebApiExample.Fakers;
using WebApiExample.Persistence;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddDbContext<FooDbContext>(options =>
{
    options.UseInMemoryDatabase("Foo");
    options.UseSeeding((context, _) =>
     {
         var foos = FooFaker.GetFaker().GenerateBetween(100, 200);
         context.Set<FooEntity>().AddRange(foos);
         context.SaveChanges();
     });
});

builder.Services.AddOpenApi();

var app = builder.Build();

var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<FooDbContext>();
dbContext.Database.EnsureCreated();

app.MapGet("/test", async (
    HttpContext context,
    [FromServices] FooDbContext dbContext,
    FooEntityOffsetPagingData data) =>
{
    var foos = await dbContext.Foos
        .ApplyFop(data, query => query.OrderBy(x => x.Id))
        .ToListAsync();

    return Results.Ok(foos);
});

app.MapOpenApi();

app.Run();