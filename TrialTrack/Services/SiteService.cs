using TrialTrack.Data;
using TrialTrack.Dtos;
using TrialTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace TrialTrack.Services;

public class SiteService 
{
    private readonly TrialTrackDbContext _db;

    public SiteService(TrialTrackDbContext db)
    {
        _db = db;
    }

    public async Task<Site> CreateSiteAsync(CreateSiteDto dto)
    {
        var site = new Site
        {
            Name = dto.Name,
            Location = dto.Location,
            Status = dto.Status,
            StudyId = dto.StudyId
        };

        _db.Sites.Add(site);
        await _db.SaveChangesAsync();

        return site;
    }
    
    public async Task<List<Site>> GetSitesAsync()
    {
        return await _db.Sites.ToListAsync();
    }
    
    public async Task<Site?> GetSiteByIdAsync(int id)
    {
        return await _db.Sites.FindAsync(id);
    }

    public async Task<List<SitePerformanceDto>> GetSitesPerformanceAsync(int studyId)
    {
        var sitesInStudy = await _db.Sites
            .Where(site => site.StudyId == studyId)
            .ToListAsync();

        var sitePerformanceResults = new List<SitePerformanceDto>();

        foreach (var site in sitesInStudy)
        {
            var everScreenedCount = await _db.Subjects
                .Where(subject =>
                    subject.ScreeningDate != null
                    && subject.SiteId == site.Id)
                .CountAsync();

            var everRandomisedCount = await _db.Subjects
                .Where(subject =>
                    subject.RandomisationDate != null
                    && subject.SiteId == site.Id)
                .CountAsync();

            var screenFailedCount = await _db.Subjects
                .Where(subject =>
                    subject.RecruitmentStatus == "Screen Failed"
                    && subject.SiteId == site.Id)
                .CountAsync();

            decimal screeningToRandomisationRate;

            if (everScreenedCount == 0)
            {
                screeningToRandomisationRate = 0;
            }
            else
            {
                screeningToRandomisationRate =
                    (decimal)everRandomisedCount / everScreenedCount * 100;
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

            var sitePerformance = new SitePerformanceDto
            {
                SiteId = site.Id,
                SiteName = site.Name,
                EverScreenedCount = everScreenedCount,
                EverRandomisedCount = everRandomisedCount,
                ScreenFailureRate = Math.Round(screenFailureRate, 2), 
                ScreeningToRandomisationRate = Math.Round(screeningToRandomisationRate, 2), 
                ScreenFailedCount = screenFailedCount
            };

            sitePerformanceResults.Add(sitePerformance);
        }

        return sitePerformanceResults;

    }

    public async Task<Site?> UpdateSiteAsync(int id, UpdateSiteDto dto)
    {
        var site = await _db.Sites.FindAsync(id);

        if (site is null)
        {
            return null;
        }

        site.Name = dto.Name;
        site.Location = dto.Location;
        site.Status = dto.Status;

        await _db.SaveChangesAsync();

        return site;
    }
    
    public async Task<bool> DeleteSiteAsync(int id)
    {
        var site = await _db.Sites.FindAsync(id);

        if (site is null)
        {
            return false;
        }

        _db.Sites.Remove(site);
        await _db.SaveChangesAsync();

        return true;
    }
    
}