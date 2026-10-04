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

    public async Task<int> GetRandomisedSubjectCountAsync()
    {
        return await _db.Subjects.Where(subject => subject.RecruitmentStatus == "Randomised")
            .CountAsync();
    }
    
    public async Task<RecruitmentSummaryDto> GetRecruitmentSummaryAsync(int studyId)
    {
        var randomisedCount = await _db.Subjects.
            Where(subject => subject.RecruitmentStatus == "Randomised" 
                             && subject.Site.StudyId == studyId)
            .CountAsync();
        
        var screeningCount = await _db.Subjects.
            Where(subject => subject.RecruitmentStatus == "Screening" 
                             && subject.Site.StudyId == studyId)
            .CountAsync();
        
        var preScreenedCount = await _db.Subjects.
            Where(subject => subject.RecruitmentStatus == "Pre-Screened" 
                             && subject.Site.StudyId == studyId)
            .CountAsync();
        
        var screenFailedCount = await _db.Subjects.
            Where(subject => subject.RecruitmentStatus == "Screen Failed"
                             && subject.Site.StudyId == studyId)
            .CountAsync();
        
        var completedCount = await _db.Subjects.
            Where(subject => subject.RecruitmentStatus == "Completed"
                             && subject.Site.StudyId == studyId)
            .CountAsync();
        
        var withdrawnCount = await _db.Subjects.
            Where(subject => subject.RecruitmentStatus == "Withdrawn"
                             && subject.Site.StudyId == studyId)
            .CountAsync();
        
        var everScreenedCount = await _db.Subjects
            .Where(subject =>
                subject.ScreeningDate != null
                && subject.Site.StudyId == studyId)
            .CountAsync();
        
        var everRandomisedCount = await _db.Subjects
            .Where(subject =>
                subject.RandomisationDate != null
                && subject.Site.StudyId == studyId)
            .CountAsync();

        decimal conversionRate;

        if (everScreenedCount == 0)
        {
            conversionRate = 0;
        }
        else
        {
            conversionRate = (decimal)everRandomisedCount / everScreenedCount * 100;
        }
        
        decimal screenFailureRate;
        if (everScreenedCount == 0)
        {
            screenFailureRate = 0;
        }
        else
        {
            screenFailureRate = (decimal)screenFailedCount / everScreenedCount * 100;
        }
        
        var summary = new RecruitmentSummaryDto
        {
            RandomisedCount = randomisedCount,
            ScreeningCount = screeningCount,
            PreScreenedCount = preScreenedCount,
            ScreenFailedCount = screenFailedCount,
            CompletedCount = completedCount,
            WithdrawnCount = withdrawnCount,
            ScreeningToRandomisationConversionRate = Math.Round(conversionRate, 2),
            ScreeningToScreenFailRate = Math.Round(screenFailureRate, 2),
        };
        
        return summary;
    }
    
    
  
}