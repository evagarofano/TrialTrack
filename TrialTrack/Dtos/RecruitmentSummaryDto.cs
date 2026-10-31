namespace TrialTrack.Dtos;

public class RecruitmentSummaryDto
{
    public int StudyId { get; set; }

    public string StudyName { get; set; } = string.Empty;
    public int RandomisedCount { get; set; }
    public int PreScreenedCount { get; set; }
    public int ScreeningCount { get; set; }
    public int ScreenFailedCount { get; set; }
    public int CompletedCount { get; set; }
    public int WithdrawnCount { get; set; }
    public decimal ScreeningToRandomisationConversionRate { get; set; }
    
    public decimal ScreeningToScreenFailRate { get; set; }
    
    public int RecruitmentTarget { get; set; }

    public decimal RecruitmentProgressPercentage { get; set; }
}