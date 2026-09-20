namespace TrialTrack.Models;

public class Study
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProtocolNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    
    public List<Site> Sites { get; set; } = new();
    
    public DateOnly? RecruitmentStartDate { get; set; }
    
    public DateOnly? ScreeningCloseDate { get; set; }
    
    public DateOnly? FinalRandomisationDate { get; set; }
    
    public int? RecruitmentTarget { get; set; }
    
    public int? ScreeningWindowDays { get; set; }
}