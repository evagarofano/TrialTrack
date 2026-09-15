namespace TrialTrack.Models;

public class Site
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int StudyId { get; set; }
    public Study Study { get; set; } = null!;
    
}