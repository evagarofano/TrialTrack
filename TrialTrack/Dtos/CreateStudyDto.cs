using System.ComponentModel.DataAnnotations;

namespace TrialTrack.Dtos;

public class CreateStudyDto
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string ProtocolNumber { get; set; } = string.Empty;

    [Required]
    public string Status { get; set; } = string.Empty;

}