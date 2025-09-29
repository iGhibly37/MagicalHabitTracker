using MagicalHabitTracker.Data;
using MagicalHabitTracker.Dto;
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


        public async Task<int> CreateTrackerAsync(int habitId, HabitTrackerDto dto)
        {
            var habit = await _appDbContext.Habits.AsNoTracking().FirstOrDefaultAsync(h => h.Id == habitId);
            if (habit == null) throw new ArgumentException("Habit not found.", nameof(habitId));

            var exists = await _appDbContext.HabitTrackers.AsNoTracking().AnyAsync(t=>t.HabitId == habitId && t.Date == dto.Date);
            if (exists) throw new InvalidOperationException("A tracker already exists for this Habit");

            var tracker = new Tracker
            {
                HabitId = habitId,
                IsCompleted = dto.IsCompleted,
                CompletedAtUtc = dto.IsCompleted ? DateTime.UtcNow : null,
                Date = dto.Date
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
                IsCompleted = t.IsCompleted,
                Date = t.Date,
                CompletedAtUtc = t.CompletedAtUtc,
            }).ToListAsync();
        }

        public async Task<List<HabitTrackerDto>> GetHistoryByHabitId(int habitId)
        {
            var trackers = await _appDbContext.HabitTrackers.AsNoTracking().Where(t => habitId == habitId)
                .OrderByDescending(t => t.Date)
                .Select(t => new HabitTrackerDto
                {
                    IsCompleted = t.IsCompleted,
                    Date = t.Date,
                    CompletedAtUtc = t.CompletedAtUtc,
                }).ToListAsync();

            return trackers ;
        }

        public async Task<HabitTrackerDto> GetTrackerAsync(int id)
        {
            var tracker = await _appDbContext.HabitTrackers.FindAsync(id);
            if(tracker == null) throw new ArgumentException("Tracker not found or already deleted.", nameof(tracker.Id));

            return new HabitTrackerDto
            {
                IsCompleted = tracker.IsCompleted,
                Date = tracker.Date,
                CompletedAtUtc = tracker.CompletedAtUtc
            };
        }

        public async Task<bool> MarkHabitCompletedAsync(int habitId, DateOnly date)
        {
            var tracker = await _appDbContext.HabitTrackers.FirstOrDefaultAsync(t => t.HabitId == habitId && t.Date == date);
            if( tracker == null) throw new ArgumentException("Tracker not found. ", nameof(tracker.Id));

            tracker.IsCompleted = true;
            tracker.CompletedAtUtc = DateTime.UtcNow;
            await _appDbContext.SaveChangesAsync();
            return true;
            
        }

        public async Task<bool> UpdateTrackerAsync(int id, HabitTrackerDto dto)
        {
            var tracker = await _appDbContext.HabitTrackers.FindAsync(id);
            if (tracker == null) throw new ArgumentException("Tracker not found or deleted. ", nameof(id));

            tracker.IsCompleted = dto.IsCompleted;
            tracker.CompletedAtUtc = dto.IsCompleted ? DateTime.UtcNow : null;

            await _appDbContext.SaveChangesAsync();
            return true;

        }
    }
}
