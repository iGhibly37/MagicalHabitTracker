using MagicalHabitTracker.Data;
using MagicalHabitTracker.Dto;
using MagicalHabitTracker.Model;
using MagicalHabitTracker.utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace MagicalHabitTracker.Service
{
    public class HabitScheduleService : IHabitScheduleService
    {

        private readonly AppDbContext _appDbContext;

        public HabitScheduleService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<int> CreateScheduleAsync(int habitId, HabitScheduleDto habitSchedDto)
        {
            var habit = await _appDbContext.Habits.AsNoTracking().FirstOrDefaultAsync(h => h.Id == habitId);
            if (habit == null)
            {
                throw new ArgumentException("Habit not found.", nameof(habit));
            }

            var exists = await _appDbContext.Schedules.AsNoTracking().AnyAsync(s=>s.HabitId == habitId);

            if (exists) throw new InvalidOperationException("A Schedule already exists for this habit.");


            if (String.IsNullOrWhiteSpace(habitSchedDto.TimeZoneId)) {throw new ArgumentException("TimeZoneId is Required.", nameof(habitSchedDto.TimeZoneId));}

            var tz = Utils.GetTimeZoneInfo(habitSchedDto.TimeZoneId);
            if (habitSchedDto.ReminderOffsetsMinutes < 0)
                throw new ArgumentException($"Reminder must be >= 0 {habitSchedDto.ReminderOffsetsMinutes}", nameof(habitSchedDto.ReminderOffsetsMinutes));
            if (habit.Periodicity == Periodicity.Weekly && habitSchedDto.WeeklyDaysMask == WeeklyDaysMask.None)
                throw new ArgumentException("WeeklyDaysMask cannot be empty for weeklyhabits.");
            if(habit.Periodicity == Periodicity.Monthly && habitSchedDto.DayOfMonth < 1 || habitSchedDto.DayOfMonth > 31)
                throw new ArgumentException("DayofMonth cannot be less or more than the range 0-31.");

            var schedule = new HabitSchedule
            {
                TimeZoneId = habitSchedDto.TimeZoneId,
                PreferredLocalTime = habitSchedDto.PreferredLocalTime,
                WeeklyDaysMask = habitSchedDto.WeeklyDaysMask,
                ReminderOffsetsMinutes = habitSchedDto.ReminderOffsetsMinutes,
                IsActive = habitSchedDto.IsActive,
                SnoozeUntilUtc = null
            };

            schedule.NextDueUtc = schedule.IsActive ? Utils.CalculateNextDueUtc(habit.Periodicity, schedule, tz, DateTime.UtcNow) : (DateTime?)null;
            _appDbContext.Schedules.Add(schedule);
            return schedule.Id;
            
        }

       
        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _appDbContext.Schedules.FindAsync(id);
            if (schedule == null) throw new ArgumentException("Schedule not found.");
            _appDbContext.Schedules.Remove(schedule);
            await _appDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<List<HabitScheduleDto>> GetAllSchedulesAsync()
        {
            return await _appDbContext.Schedules.AsNoTracking()
                .Select(s => new HabitScheduleDto
                {
                    TimeZoneId = s.TimeZoneId,
                    PreferredLocalTime = s.PreferredLocalTime,
                    WeeklyDaysMask = s.WeeklyDaysMask,
                    ReminderOffsetsMinutes = s.ReminderOffsetsMinutes,
                    IsActive = s.IsActive,
                    NextDueUtc = s.NextDueUtc
                }).ToListAsync();
                 
        }

        public async Task<HabitScheduleDto?> GetScheduleByIdAsync(int id)
        {
            var habit = await _appDbContext.Schedules.FindAsync(id);
            if(habit == null) return null;

            return new HabitScheduleDto
            {
                TimeZoneId = habit.TimeZoneId,
                PreferredLocalTime = habit.PreferredLocalTime,
                WeeklyDaysMask = habit.WeeklyDaysMask,
                ReminderOffsetsMinutes = habit.ReminderOffsetsMinutes,
                IsActive = habit.IsActive,
                NextDueUtc = habit.NextDueUtc
            };

        }

        public async Task<bool> UpdateScheduleAsync(int id, HabitScheduleDto habitSchedDto)
        {
            var existingSchedule = await _appDbContext.Schedules.FindAsync(id);
            if (existingSchedule == null)
                throw new ArgumentException("Schedule not found. Re-try again.", nameof(existingSchedule));

            if (habitSchedDto.ReminderOffsetsMinutes < 0)
                return false;

            existingSchedule.TimeZoneId = habitSchedDto.TimeZoneId;
            existingSchedule.IsActive = habitSchedDto.IsActive;
            existingSchedule.PreferredLocalTime = habitSchedDto.PreferredLocalTime;
            existingSchedule.WeeklyDaysMask = habitSchedDto.WeeklyDaysMask;
            existingSchedule.SnoozeUntilUtc = null;
            existingSchedule.ReminderOffsetsMinutes = habitSchedDto.ReminderOffsetsMinutes;


            var periodicity = await _appDbContext.Habits.Where(h => h.Id == existingSchedule.HabitId)
                .Select(h => h.Periodicity)
                .FirstOrDefaultAsync();

            existingSchedule.NextDueUtc = existingSchedule.IsActive ? Utils.CalculateNextDueUtc(periodicity, existingSchedule, Utils.GetTimeZoneInfo(existingSchedule.TimeZoneId), DateTime.UtcNow) : (DateTime?)null;

            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
