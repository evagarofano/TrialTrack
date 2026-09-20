using System.ComponentModel.DataAnnotations;
namespace TrialTrack.Models;

public class Subject
{
    public int Id { get; set; }

    [Required] 
    public string SubjectNumber { get; set; } = "";

    public int SiteId { get; set; }
    public Site Site { get; set; } = null!;

    [Required] 
    public string RecruitmentStatus { get; set; } = "";

    public DateOnly? PreScreenDate { get; set; }
    public DateOnly? ScreeningDate { get; set; }
    public DateOnly? RandomisationDate { get; set; }
    public DateOnly? ScreenFailDate { get; set; }
    public string? ScreenFailReason { get; set; }
    
}