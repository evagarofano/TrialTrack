using Microsoft.EntityFrameworkCore;
using TrialTrack.Data;
using TrialTrack.Models;
using TrialTrack.Dtos;

namespace TrialTrack.Services;

public class StudyService
{
    private readonly TrialTrackDbContext _db;

    public StudyService(TrialTrackDbContext db)
    {
        _db = db;
    }
    public async Task<bool> ProtocolNumberExistsAsync(string protocolNumber)
    {
        return await _db.Studies
            .AnyAsync(study => study.ProtocolNumber == protocolNumber);
    }
    
    public async Task<Study> CreateStudyAsync(CreateStudyDto dto)
    {
        var study = new Study
        {
            Name = dto.Name,
            ProtocolNumber = dto.ProtocolNumber,
            Status = dto.Status
        };

        _db.Studies.Add(study);

        await _db.SaveChangesAsync();

        return study;
    }
    
    public async Task<List<Study>> GetStudiesAsync()
    {
        return await _db.Studies.ToListAsync();
    }
    
    public async Task<Study?> GetStudyByIdAsync(int id)
    {
        return await _db.Studies.FindAsync(id);
    }
    
    public async Task<Study?> UpdateStudyAsync(int id, UpdateStudyDto dto)
    {
        var study = await _db.Studies.FindAsync(id);

        if (study is null)
        {
            return null;
        }
        study.Name = dto.Name;
        study.Status = dto.Status;
        await _db.SaveChangesAsync();
        
        return study;
    }
    
    public async Task<bool> DeleteStudyAsync(int id)
    {
        var study = await _db.Studies.FindAsync(id);
        
        if (study is null)
        {
            return false;
        }
        
        _db.Studies.Remove(study);
        
        await _db.SaveChangesAsync();
        
        return true;
    }
}