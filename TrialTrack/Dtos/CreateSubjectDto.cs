using System.ComponentModel.DataAnnotations;

namespace TrialTrack.Dtos;

public class CreateSubjectDto
{
    [Required]
    public string SubjectNumber { get; set; } = string.Empty;
    [Range(1, int.MaxValue)]
    public int SiteId { get; set; }
    [Required]
    public string RecruitmentStatus { get; set; } = string.Empty;
    
    public DateOnly? PreScreenDate { get; set; } 
    
    public DateOnly? ScreeningDate { get; set; }
    
    public DateOnly? RandomisationDate { get; set; }
    
    public DateOnly? ScreenFailDate { get; set; }
    
    public string? ScreenFailReason { get; set; } 
}