namespace TrialTrack.Models;

public class Study
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ProtocolNumber { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}