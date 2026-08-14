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

app.Run();
