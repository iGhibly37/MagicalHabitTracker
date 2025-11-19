using MagicalHabitTracker.Data;
using MagicalHabitTracker.Dto.HabitTrackerDtos;
using MagicalHabitTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace MagicalHabitTracker.Service
{
    public class TrackerService : ITrackerService
    {
        private readonly AppDbContext _appDbContext;

        public TrackerService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<int> CreateTrackerAsync(int habitId, CreateHabitTrackerDto dto)
        {
            var habit = await _appDbContext.Habits.AsNoTracking().FirstOrDefaultAsync(h => h.Id == habitId);
            if (habit == null) throw new ArgumentException("Habit not found.", nameof(habitId));

            var exists = await _appDbContext.HabitTrackers.AsNoTracking().AnyAsync(t=>t.HabitId == habitId && t.CompletedAtUtc == dto.CompletedAtUtc);
            if (exists) throw new InvalidOperationException("A tracker already exists for this Habit");

            var tracker = new HabitTracker
            {
                HabitId = habitId,
            };

            _appDbContext.HabitTrackers.Add(tracker);
            await _appDbContext.SaveChangesAsync();
            return tracker.Id;     
        }

        public async Task<bool> DeletTrackerAsync(int id)
        {
            var tracker = await _appDbContext.HabitTrackers.FindAsync(id);
            if(tracker == null) throw new ArgumentException("Tracker not found or already deleted.", nameof(tracker.Id));
            _appDbContext.HabitTrackers.Remove(tracker);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<HabitTrackerDto>> GetAll()
        {
            return await _appDbContext.HabitTrackers.AsNoTracking().Select(t => new HabitTrackerDto
            {
                CompletedAtUtc = t.CompletedAtUtc,
                HabitId = t.HabitId,
                Id = t.Id
            }).ToListAsync();
        }

        public async Task<List<HabitTrackerDto>> GetHistoryByHabitId(int habitId)
        {
            var trackers = await _appDbContext.HabitTrackers.AsNoTracking().Where(t => habitId == habitId)
                .OrderByDescending(t => t.CompletedAtUtc)
                .Select(t => new HabitTrackerDto
                {
                    CompletedAtUtc = t.CompletedAtUtc,
                }).ToListAsync();

            return trackers ;
        }

        public async Task<HabitTrackerDto> GetTrackerAsync(int id)
        {
            var tracker = await _appDbContext.HabitTrackers.Where(t => t.Id == id).Select(t => new HabitTrackerDto
            {
                CompletedAtUtc = t.CompletedAtUtc,
                Id = t.Id,
                HabitId = t.HabitId
            }).FirstOrDefaultAsync();

            if (tracker == null) throw new ArgumentException("Tracker not found or already deleted.", nameof(tracker.Id));
            
            return tracker;
        }

        public async Task<bool> MarkHabitCompletedAsync(int habitId, DateOnly date)
        {
            var tracker = await _appDbContext.HabitTrackers.FirstOrDefaultAsync(t => t.HabitId == habitId);
            if( tracker == null) throw new ArgumentException("Tracker not found. ", nameof(tracker.Id));

            if(tracker.CompletedAtUtc != null)
            {
                return false;
            }

            tracker.CompletedAtUtc = DateTime.UtcNow;
            await _appDbContext.SaveChangesAsync();
            return true;
            
        }

        //public async Task<bool> UpdateTrackerAsync(int id, HabitTrackerDto dto)
        //{
        //    var tracker = await _appDbContext.HabitTrackers.Where(t => t.Id == id).Select(ht => new HabitTrackerDto
        //    {
        //        CompletedAtUtc = ht.CompletedAtUtc,
        //        HabitId = ht.HabitId,
        //        Id = ht.Id
        //    }).FirstOrDefaultAsync();

        //    if (tracker == null) throw new ArgumentException("Tracker not found or deleted. ", nameof(id));

        //    tracker.CompletedAtUtc = DateTime.UtcNow;

        //    await _appDbContext.SaveChangesAsync();
        //    return true;

        //}
    }
}
