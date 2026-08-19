using TrialTrack.Models;
using TrialTrack.Dtos;
using Microsoft.EntityFrameworkCore;
using TrialTrack.Data;
using TrialTrack.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<TrialTrackDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TrialTrack")
    )
);

builder.Services.AddScoped<StudyService>();

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

app.MapGet("/studies", async (StudyService studyService) =>
{
    var studies = await studyService.GetStudiesAsync();

    return Results.Ok(studies);
});

app.MapGet("/studies/{id}", async (int id, StudyService studyService) =>
{
    var study = await studyService.GetStudyByIdAsync(id);

    if (study is null)
    {
        return Results.NotFound();
    }
    
    return Results.Ok(study);
});

app.MapPost("/studies", async (
    CreateStudyDto dto,
    StudyService studyService) =>
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
    
    var protocolExists =
        await studyService.ProtocolNumberExistsAsync(dto.ProtocolNumber);
    
    if (protocolExists)
    {
        return Results.BadRequest(
            "A study with this protocol number already exists.");
    }
    
    var study = await studyService.CreateStudyAsync(dto);

    return Results.Created($"/studies/{study.Id}", study);
});

app.MapPut("/studies/{id}", async (int id, UpdateStudyDto dto, StudyService studyService) =>
{
    var study = await studyService.UpdateStudyAsync(id, dto);

    if (study is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(study);
});

app.MapDelete("/studies/{id}", async (int id, StudyService studyService) =>
{
    var deleted = await studyService.DeleteStudyAsync(id);
    
    if (!deleted)
    {
        return Results.NotFound();
    }
    
    return Results.NoContent();
});

app.Run();
