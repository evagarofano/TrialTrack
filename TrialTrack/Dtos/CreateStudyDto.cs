namespace TrialTrack.Dtos;

public record CreateStudyDto(
    string Name,
    string ProtocolNumber,
    string Status
);