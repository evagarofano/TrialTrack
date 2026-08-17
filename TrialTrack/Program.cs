using TrialTrack.Models;
using TrialTrack.Dtos;
using Microsoft.EntityFrameworkCore;
using TrialTrack.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<TrialTrackDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TrialTrack")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

//Commented as DB linked up
// var studies = new List<Study>
// {
//     new Study
//     {
//         Id = 1,
//         Name = "Heart Health Study",
//         ProtocolNumber = "CV-001",
//         Status = "Planning"
//     },
//     new Study
//     {
//         Id = 2,
//         Name = "Weight Management Study",
//         ProtocolNumber = "WM-002",
//         Status = "Recruiting"
//     }
// };

app.MapGet("/studies", async (TrialTrackDbContext db) =>
{
    var studies = await db.Studies.ToListAsync();

    return Results.Ok(studies);
});

app.MapGet("/studies/{id}", async (int id, TrialTrackDbContext db) =>
{
    var study = await db.Studies.FindAsync(id);

    if (study is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(study);
});

app.MapPost("/studies", async (CreateStudyDto dto, TrialTrackDbContext db) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name) ||
        string.IsNullOrWhiteSpace(dto.ProtocolNumber) ||
        string.IsNullOrWhiteSpace(dto.Status))
    {
        return Results.BadRequest("Name, protocol number and status are required.");
    }

    var allowedStatuses = new[] { "Planning", "Recruiting", "Active", "Closed" };

    if (!allowedStatuses.Contains(dto.Status))
    {
        return Results.BadRequest("Status must be Planning, Recruiting, Active or Closed.");
    }
    
    var protocolExists = await db.Studies
        .AnyAsync(s => s.ProtocolNumber == dto.ProtocolNumber);

    if (protocolExists)
    {
        return Results.BadRequest(
            "A study with this protocol number already exists.");
    }
    
    var newStudy = new Study
    {
        Name = dto.Name,
        ProtocolNumber = dto.ProtocolNumber,
        Status = dto.Status
    };
    
    db.Studies.Add(newStudy);

    await db.SaveChangesAsync();

    return Results.Created($"/studies/{newStudy.Id}", newStudy);
});

app.MapPut("/studies/{id}", async (int id, UpdateStudyDto dto, TrialTrackDbContext db) =>
{
    var study = await db.Studies.FindAsync(id);

    if (study is null)
    {
        return Results.NotFound();
    }

    study.Name = dto.Name;
    study.Status = dto.Status;

    await db.SaveChangesAsync();

    return Results.Ok(study);
});

app.MapDelete("/studies/{id}", async (int id, TrialTrackDbContext db) =>
{
    var study = await db.Studies.FindAsync(id);

    if (study is null)
    {
        return Results.NotFound();
    }

    db.Studies.Remove(study);

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
