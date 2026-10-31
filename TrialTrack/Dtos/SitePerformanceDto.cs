namespace TrialTrack.Dtos;

public class SitePerformanceDto
{
    public int SiteId { get; set; }
    public string SiteName { get; set; } = string.Empty;
    public int EverScreenedCount { get; set; }
    public int EverRandomisedCount { get; set; }
    public int ScreenFailedCount { get; set; }
    public decimal ScreeningToRandomisationRate { get; set; }
    public decimal ScreenFailureRate { get; set; }
    
    public int RecruitmentTarget { get; set; }

    public decimal RecruitmentProgressPercentage { get; set; }
    
    public int? MonthlyRecruitmentTarget { get; set; }
    
    public int MonthlyRandomisedCount { get; set; }
    
    public decimal? MonthlyRecruitmentProgressPercentage { get; set; }
}