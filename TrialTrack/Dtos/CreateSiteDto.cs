using System.ComponentModel.DataAnnotations;

namespace TrialTrack.Dtos;

public class CreateSiteDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Location { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = string.Empty;
    
    [Range(1, int.MaxValue)]
    public int StudyId { get; set; }
}