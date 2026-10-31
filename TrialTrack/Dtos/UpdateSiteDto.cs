using System.ComponentModel.DataAnnotations;

namespace TrialTrack.Dtos;

public class UpdateSiteDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = string.Empty;
    
    public int RecruitmentTarget { get; set; }
    
    [Range(0, int.MaxValue)]
    public int? MonthlyRecruitmentTarget { get; set; }
}