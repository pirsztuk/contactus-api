using ContactUsApi.Models;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

Env.Load();
var accessToken = Environment.GetEnvironmentVariable("ACCESS_TOKEN");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.MapGet("/", () => "Api is working");

app.MapGet("/contacts", async (HttpContext context, ApplicationDbContext db) =>
{
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

    if (token != accessToken)
    {
        return Results.Unauthorized();
    }

    var contacts = await db.ContactForms.ToListAsync();
    return Results.Ok(contacts);
});

app.MapPost("/contact", async (HttpContext context, ContactForm form, ApplicationDbContext db) =>
{
    var ip = context.Connection.RemoteIpAddress?.ToString();

    form.Ip = ip;

    db.ContactForms.Add(form);
    await db.SaveChangesAsync();

    return Results.Ok(new { form.Id });
});

app.MapDelete("/contacts/{id}", async (int id, HttpContext context, ApplicationDbContext db) =>
{
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

    if (token != accessToken)
    {
        return Results.Unauthorized();
    }

    var contact = await db.ContactForms.FindAsync(id);
    if (contact == null)
    {
        return Results.NotFound();
    }

    db.ContactForms.Remove(contact);
    await db.SaveChangesAsync();

    return Results.Ok(new { message = "Deleted successfully", contact.Id });
});

app.Run();
