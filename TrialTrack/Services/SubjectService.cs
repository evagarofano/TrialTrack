using TrialTrack.Data;
using TrialTrack.Dtos;
using TrialTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace TrialTrack.Services;

public class SubjectService
{
    private readonly TrialTrackDbContext _db;

    public SubjectService(TrialTrackDbContext db)
    {
        _db = db;
    }
    
    public async Task<bool> SubjectNumberExistsAsync(string subjectNumber)
    {
        return await _db.Subjects
            .AnyAsync(subject => subject.SubjectNumber == subjectNumber);
    }

    public async Task<Subject> CreateSubjectAsync(CreateSubjectDto dto)
    {
        var subject = new Subject
        {
            SubjectNumber = dto.SubjectNumber,
            SiteId = dto.SiteId,
            RecruitmentStatus = dto.RecruitmentStatus,
            PreScreenDate = dto.PreScreenDate,
            ScreeningDate = dto.ScreeningDate,
            RandomisationDate = dto.RandomisationDate,
            ScreenFailDate = dto.ScreenFailDate,
            ScreenFailReason = dto.ScreenFailReason
        };
        
        _db.Subjects.Add(subject);
        
        await _db.SaveChangesAsync();
        
        return subject;
    }
    
    public async Task<List<Subject>> GetSubjectsAsync()
    {
        return await _db.Subjects.ToListAsync();
    }
    
    public async Task<Subject?> GetSubjectByIdAsync(int id)
    {
        return await _db.Subjects.FindAsync(id);
    }
    
    public async Task<Subject?> UpdateSubjectAsync(int id, UpdateSubjectDto dto)
    {
        var subject = await _db.Subjects.FindAsync(id);

        if (subject is null)
        {
            return null;
        }

        subject.SubjectNumber = dto.SubjectNumber;
        subject.SiteId = dto.SiteId;
        subject.RecruitmentStatus = dto.RecruitmentStatus;
        subject.PreScreenDate = dto.PreScreenDate;
        subject.ScreeningDate = dto.ScreeningDate;
        subject.RandomisationDate = dto.RandomisationDate;
        subject.ScreenFailDate = dto.ScreenFailDate;
        subject.ScreenFailReason = dto.ScreenFailReason;
            
        await _db.SaveChangesAsync();
        
        return subject;
    }
    
    public async Task<bool> DeleteSubjectAsync(int id)
    {
        var subject = await _db.Subjects.FindAsync(id);
        
        if (subject is null)
        {
            return false;
        }
        
        _db.Subjects.Remove(subject);
        
        await _db.SaveChangesAsync();
        
        return true;
    }
}