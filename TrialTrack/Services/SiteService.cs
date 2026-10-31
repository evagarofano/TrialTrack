using TrialTrack.Data;
using TrialTrack.Dtos;
using TrialTrack.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
            StudyId = dto.StudyId,
            RecruitmentTarget = dto.RecruitmentTarget,
            MonthlyRecruitmentTarget = dto.MonthlyRecruitmentTarget,
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
        
        var startOfMonth = new DateOnly(
            DateTime.UtcNow.Year,
            DateTime.UtcNow.Month,
            1);

        var startOfNextMonth = startOfMonth.AddMonths(1);

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
            
            var monthlyRandomisedCount = await _db.Subjects
                .CountAsync(subject =>
                    subject.SiteId == site.Id &&
                    subject.RandomisationDate >= startOfMonth &&
                    subject.RandomisationDate < startOfNextMonth);

            decimal? monthlyRecruitmentProgressPercentage = null;

            if (site.MonthlyRecruitmentTarget > 0)
            {
                monthlyRecruitmentProgressPercentage = Math.Round(
                    (decimal)monthlyRandomisedCount /
                    site.MonthlyRecruitmentTarget.Value * 100, 2);
            }
            
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

            decimal recruitmentProgressPercentage = 0;
            if (site.RecruitmentTarget > 0)
            {
                recruitmentProgressPercentage =  Math.Round((decimal)everRandomisedCount / site.RecruitmentTarget * 100, 2);
            }
            
            var sitePerformance = new SitePerformanceDto
            {
                SiteId = site.Id,
                SiteName = site.Name,
                EverScreenedCount = everScreenedCount,
                EverRandomisedCount = everRandomisedCount,
                ScreenFailureRate = Math.Round(screenFailureRate, 2), 
                ScreeningToRandomisationRate = Math.Round(screeningToRandomisationRate, 2), 
                ScreenFailedCount = screenFailedCount,
                RecruitmentTarget = site.RecruitmentTarget,
                RecruitmentProgressPercentage = recruitmentProgressPercentage,
                MonthlyRecruitmentTarget = site.MonthlyRecruitmentTarget,
                MonthlyRandomisedCount = monthlyRandomisedCount,
                MonthlyRecruitmentProgressPercentage = monthlyRecruitmentProgressPercentage,
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
        site.RecruitmentTarget = dto.RecruitmentTarget;
        site.MonthlyRecruitmentTarget = dto.MonthlyRecruitmentTarget;

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