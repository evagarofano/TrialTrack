using Microsoft.AspNetCore.Http.HttpResults;
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
builder.Services.AddScoped<SiteService>();
builder.Services.AddScoped<SubjectService>();

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

app.MapPost("/sites", async (
    CreateSiteDto dto,
    StudyService studyService,
    SiteService siteService
) =>
{
    
    var study = await studyService.GetStudyByIdAsync(dto.StudyId);

    if (study is null)
    {
        return Results.NotFound();
    }

    var newSite = await siteService.CreateSiteAsync(dto);

    return Results.Created(
        $"/sites/{newSite.Id}",
        new
        {
            newSite.Id,
            newSite.Name,
            newSite.Location,
            newSite.Status,
            newSite.StudyId
        });
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

app.MapGet("/sites/{id}", async (int id, SiteService siteService) =>
{
    var site = await siteService.GetSiteByIdAsync(id);

    if (site is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(site);
});

app.MapGet("/sites", async (SiteService siteService) =>
{
    var sites = await siteService.GetSitesAsync();

    return Results.Ok(sites);
});


app.MapPut("/sites/{id}", async (
    int id,
    UpdateSiteDto dto,
    SiteService siteService) =>
{
    if (string.IsNullOrWhiteSpace(dto.Name) ||
        string.IsNullOrWhiteSpace(dto.Location) ||
        string.IsNullOrWhiteSpace(dto.Status))
    {
        return Results.BadRequest(
            "Name, Location and Status are required.");
    }

    var allowedStatuses = new[]
    {
        "Pending", "Awarded", "Recruiting", "Active", "Closed"
    };

    if (!allowedStatuses.Contains(dto.Status))
    {
        return Results.BadRequest(
            "Status must be Pending, Awarded, Recruiting, Active or Closed.");
    }

    var site = await siteService.UpdateSiteAsync(id, dto);

    if (site is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(site);
});

app.MapDelete("/sites/{id}", async (
    int id,
    SiteService siteService) =>
{
    var deleted = await siteService.DeleteSiteAsync(id);

    if (!deleted)
    {
        return Results.NotFound();
    }
    
    return Results.NoContent();

});

app.MapPost("/subjects", async (
    CreateSubjectDto dto,
    SubjectService subjectService,
    SiteService siteService
) =>
{
    
    if (dto.SiteId <= 0 ||
        string.IsNullOrWhiteSpace(dto.SubjectNumber) ||
        string.IsNullOrWhiteSpace(dto.RecruitmentStatus))
    {
        return Results.BadRequest("SiteId, Subject Number and Recruitment Status are required.");
    }
        
    var allowedStatuses = new[]
    {
        "Pre-Screened", 
        "Screening",
        "Randomised",
        "Screen Failed",
        "Completed",
        "Withdrawn"
    };
    
    if (!allowedStatuses.Contains(dto.RecruitmentStatus))
    {
        return Results.BadRequest(
            "Recruitment Status must be Pre-Screened, Screening, Randomised, Screen Failed, Completed or Withdrawn.");
    }
    
    var site = await siteService.GetSiteByIdAsync(dto.SiteId);
    
    if (site is null)
    {
        return Results.NotFound();
    }
    
    var subjectNumberExists =
        await subjectService.SubjectNumberExistsAsync(dto.SubjectNumber);

    if (subjectNumberExists)
    {
        return Results.BadRequest(
            "A subject with this subject number already exists.");
    }
    
    if (dto.RecruitmentStatus == "Randomised" &&
        dto.RandomisationDate is null)
    {
        return Results.BadRequest(
            "Randomisation Date is required when a subject is Randomised.");
    }
    
    var newSubject = await subjectService.CreateSubjectAsync(dto);

    return Results.Created(
        $"/subjects/{newSubject.Id}",
        new
        {
            newSubject.Id,
            newSubject.SiteId,
            newSubject.SubjectNumber,
            newSubject.RandomisationDate,
            newSubject.RecruitmentStatus,
            newSubject.ScreeningDate,
            newSubject.PreScreenDate,
            newSubject.ScreenFailDate,
            newSubject.ScreenFailReason
        });
    
});

app.MapGet("/subjects", async (SubjectService subjectService) =>
{
    var subjects = await subjectService.GetSubjectsAsync();

    return Results.Ok(subjects);
});

app.MapGet("/subjects/{id}", async (
    int id,
    SubjectService subjectService) =>
{
    var subject = await subjectService.GetSubjectByIdAsync(id);

    if (subject is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(subject);
});

app.MapPut("/subjects/{id}", async (
    int id,
    UpdateSubjectDto dto,
    SubjectService subjectService,
    SiteService siteService
) =>
{
    var subject = await subjectService.GetSubjectByIdAsync(id);

    if (subject is null)
    {
        return Results.NotFound();
    }

    if (dto.SiteId <= 0 ||
        string.IsNullOrWhiteSpace(dto.SubjectNumber) ||
        string.IsNullOrWhiteSpace(dto.RecruitmentStatus))
    {
        return Results.BadRequest(
            "SiteId, Subject Number and Recruitment Status are required.");
    }

    var allowedStatuses = new[]
    {
        "Pre-Screened",
        "Screening",
        "Randomised",
        "Screen Failed",
        "Completed",
        "Withdrawn"
    };

    if (!allowedStatuses.Contains(dto.RecruitmentStatus))
    {
        return Results.BadRequest(
            "Recruitment Status is invalid.");
    }

    var site = await siteService.GetSiteByIdAsync(dto.SiteId);
    
    if (site is null)
    {
        return Results.NotFound();
    }

    if (dto.SubjectNumber != subject.SubjectNumber)
    {
        var subjectNumberExists = 
            await subjectService.SubjectNumberExistsAsync(dto.SubjectNumber);

        if (subjectNumberExists)
        {
            return Results.BadRequest("A subject with this subject number already exists.");
        }
    }

    if (dto.RecruitmentStatus == "Randomised" 
        && dto.RandomisationDate is null)
    {
        return Results.BadRequest("Randomisation Date is required when a subject is Randomised.");
    }

    var updatedSubject = 
        await subjectService.UpdateSubjectAsync(id, dto);
       
    if (updatedSubject is null)
            {
                return Results.NotFound();
            }
    
    return Results.Ok(updatedSubject);
});

app.MapDelete("/subjects/{id}", async (
    int id,
    SubjectService subjectService) =>
{
    var deleted = await subjectService.DeleteSubjectAsync(id);

    if (!deleted)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

app.Run();
