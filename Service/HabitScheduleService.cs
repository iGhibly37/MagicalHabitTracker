using MagicalHabitTracker.Data;
using MagicalHabitTracker.Dto.ScheduleDtos;
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

        public async Task<int> CreateScheduleAsync(int habitId, CreateHabitScheduleDto habitSchedDto)
        {
            var habit = await _appDbContext.Habits.AsNoTracking().FirstOrDefaultAsync(h => h.Id == habitId);
           

            if (habit is null) throw new ArgumentException("Habit not found.", nameof(habit));
            
            bool scheduleExists = await _appDbContext.Schedules.AsNoTracking().AnyAsync(s=>s.HabitId == habitId);
           

            if (scheduleExists) throw new InvalidOperationException("A Schedule already exists for this habit.");


            if (String.IsNullOrWhiteSpace(habitSchedDto.TimeZoneId)) {throw new ArgumentException("TimeZoneId is Required.", nameof(habitSchedDto.TimeZoneId));}

            var tz = Utils.GetTimeZoneInfo(habitSchedDto.TimeZoneId);

            if (habitSchedDto.ReminderOffsetsMinutes < 0)
                throw new ArgumentException($"Reminder must be >= 0 {habitSchedDto.ReminderOffsetsMinutes}", nameof(habitSchedDto.ReminderOffsetsMinutes));
            if (habit.Periodicity == Periodicity.Weekly && habitSchedDto.WeeklyDaysMask == WeeklyDaysMask.None)
                throw new ArgumentException("WeeklyDaysMask cannot be empty for weeklyhabits.");

            var newSchedule = new HabitSchedule
            {
                HabitId = habitId,
                TimeZoneId = habitSchedDto.TimeZoneId,
                PreferredLocalTime = habitSchedDto.PreferredLocalTime,
                WeeklyDaysMask = habitSchedDto.WeeklyDaysMask,
                ReminderOffsetsMinutes = habitSchedDto.ReminderOffsetsMinutes,
                IsActive = habitSchedDto.IsActive,
                SnoozeUntilUtc = null,
            };

            newSchedule.NextDueUtc = newSchedule.IsActive ? Utils.CalculateNextDueUtc(habit.Periodicity, newSchedule, tz, DateTime.UtcNow) : (DateTime?)null;
            _appDbContext.Schedules.Add(newSchedule);
            await _appDbContext.SaveChangesAsync();
            return newSchedule.Id;
            
        }

       
        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _appDbContext.Schedules.FindAsync(id);
            if (schedule == null) throw new ArgumentException("Schedule not found.");
            _appDbContext.Schedules.Remove(schedule);
            await _appDbContext.SaveChangesAsync();
            return true;

        }

        public async Task<List<GetHabitScheduleDto>> GetAllSchedulesAsync()
        {
            return await _appDbContext.Schedules.AsNoTracking()
                .Select(s => new GetHabitScheduleDto
                {
                    TimeZoneId = s.TimeZoneId,
                    PreferredLocalTime = s.PreferredLocalTime,
                    WeeklyDaysMask = s.WeeklyDaysMask,
                    IsActive = s.IsActive,
                    NextDueUtc = s.NextDueUtc
                }).ToListAsync();
                 
        }

        public async Task<GetHabitScheduleDto?> GetScheduleByIdAsync(int id)
        {
            var habit = await _appDbContext.Schedules.FindAsync(id);
            if(habit == null) return null;

            return new GetHabitScheduleDto
            {
                TimeZoneId = habit.TimeZoneId,
                PreferredLocalTime = habit.PreferredLocalTime,
                WeeklyDaysMask = habit.WeeklyDaysMask,
                IsActive = habit.IsActive,
                NextDueUtc = habit.NextDueUtc
            };

        }

        public async Task<bool> PutScheduleAsync(int id, UpdateHabitScheduleDto habitSchedDto)
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

        public async Task<bool> PatchScheduleAsync(int id, PatchScheduleDto updateScheduleDto)
        {
            var existingSchedule = await _appDbContext.Schedules.FindAsync(id);

            if(existingSchedule == null)
            {
                throw new ArgumentException("Schedule not found.", nameof(existingSchedule));
            }

            if (updateScheduleDto.ReminderOffsetsMinutes.HasValue && updateScheduleDto.ReminderOffsetsMinutes.Value < 0)
                return false;



            if (updateScheduleDto.TimeZoneId is not null)
                existingSchedule.TimeZoneId = updateScheduleDto.TimeZoneId;

            if (updateScheduleDto.IsActive.HasValue)
                existingSchedule.IsActive = updateScheduleDto.IsActive.Value;

            if (updateScheduleDto.PreferredLocalTime.HasValue)
                existingSchedule.PreferredLocalTime = updateScheduleDto.PreferredLocalTime.Value;

            if (updateScheduleDto.WeeklyDaysMask != WeeklyDaysMask.AllDays)
                existingSchedule.WeeklyDaysMask = updateScheduleDto.WeeklyDaysMask;

            if (updateScheduleDto.ReminderOffsetsMinutes.HasValue)
                existingSchedule.ReminderOffsetsMinutes = updateScheduleDto.ReminderOffsetsMinutes.Value;

            
            existingSchedule.SnoozeUntilUtc = null;

            
            var periodicity = await _appDbContext.Habits
                .Where(h => h.Id == existingSchedule.HabitId)
                .Select(h => h.Periodicity)
                .FirstOrDefaultAsync();

            existingSchedule.NextDueUtc = existingSchedule.IsActive
                ? Utils.CalculateNextDueUtc(
                    periodicity,
                    existingSchedule,
                    Utils.GetTimeZoneInfo(existingSchedule.TimeZoneId),
                    DateTime.UtcNow)
                : (DateTime?)null;

            await _appDbContext.SaveChangesAsync();
            return true;
        }

    }
}
