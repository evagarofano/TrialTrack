using TrialTrack.Models;
using TrialTrack.Dtos;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var studies = new List<Study>
{
    new Study
    {
        Id = 1,
        Name = "Heart Health Study",
        ProtocolNumber = "CV-001",
        Status = "Planning"
    },
    new Study
    {
        Id = 2,
        Name = "Weight Management Study",
        ProtocolNumber = "WM-002",
        Status = "Recruiting"
    }
};

app.MapGet("/studies", () =>
{
    return studies;
});

app.MapGet("/studies/{id}", (int id) =>
{
    var study = studies.FirstOrDefault(s => s.Id == id);
    
    if (study is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(study);
});

app.MapPost("/studies", (CreateStudyDto dto) =>
{
    var newStudy = new Study
    {
        Id = studies.Count + 1,
        Name = dto.Name,
        ProtocolNumber = dto.ProtocolNumber,
        Status = dto.Status
    };
    
    studies.Add(newStudy);

    return Results.Created($"/studies/{newStudy.Id}", newStudy);
});

app.Run();
