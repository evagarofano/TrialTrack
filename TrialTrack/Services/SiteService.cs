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
    
    public List<Subject> Subjects { get; set; } = new();
}