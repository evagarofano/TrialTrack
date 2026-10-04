namespace TrialTrack.Dtos;

public class RecruitmentSummaryDto
{
    public int RandomisedCount { get; set; }
    public int PreScreenedCount { get; set; }
    public int ScreeningCount { get; set; }
    public int ScreenFailedCount { get; set; }
    public int CompletedCount { get; set; }
    public int WithdrawnCount { get; set; }
}