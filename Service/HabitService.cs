using MagicalHabitTracker.Data;
using MagicalHabitTracker.Dto.HabitDtos;
using MagicalHabitTracker.Dto.ScheduleDtos;
using MagicalHabitTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace MagicalHabitTracker.Service
{
    public class HabitService : IHabitService
    {

        private readonly AppDbContext appDbContext;


        public HabitService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<int> AddHabitAsync(CreateHabitDto habitDto, int userId)
        {
            if (String.IsNullOrWhiteSpace(habitDto.Name))
            {
                throw new ArgumentException("Habit name cannot be null or empty.");
            }

            if (String.IsNullOrWhiteSpace(habitDto.Name.Trim()))
            {
                throw new ArgumentException("Habit name cannot be empty or whitespace.");
            }


            Habit habit = new Habit
            {
                UserId = userId,
                Name = habitDto.Name,
                Description = habitDto.Description,
                Periodicity = habitDto.Periodicity,
                TargetPeriod = habitDto.TargetPeriod,
                CreatedAt = DateTime.UtcNow,
                //HabitSchedule = habitDto.Schedule != null ? new HabitSchedule
                //{
                //    WeeklyDaysMask = habitDto.Schedule.WeeklyDaysMask,
                //    TimeZoneId = habitDto.Schedule.TimeZoneId,
                //    PreferredLocalTime = habitDto.Schedule.PreferredLocalTime,
                //    IsActive = habitDto.Schedule.IsActive,
                //    NextDueUtc = habitDto.Schedule.NextDueUtc
                //} : null
            };
            appDbContext.Habits.Add(habit);
            await appDbContext.SaveChangesAsync();
            return habit.Id;
        }

        public async Task<bool> DeleteHabitAsync(int id)
        {
            var habit = await appDbContext.Habits.FindAsync(id);
            if (habit == null || habit.Id <= 0)
                throw new ArgumentException("Invalid habit.");


            appDbContext.Habits.Remove(habit);
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<GetHabitDto>> GetAllHabitsAsync(int UserId)
        {
            return await appDbContext.Habits.Where(h=> h.UserId == UserId).OrderBy(h => h.IsArchived)
                 .ThenBy(h => h.Name)
                 .AsNoTracking()
                 .Select(h => new GetHabitDto
                 {
                     Id = h.Id,
                     Name = h.Name,
                     Description = h.Description,
                     TargetPeriod = h.TargetPeriod,
                     CreatedAt = h.CreatedAt,
                     HabitSchedule = h.HabitSchedule != null ? new GetHabitScheduleDto
                     {
                         WeeklyDaysMask = h.HabitSchedule.WeeklyDaysMask,
                         TimeZoneId = h.HabitSchedule.TimeZoneId,
                         PreferredLocalTime = h.HabitSchedule.PreferredLocalTime,
                         IsActive = h.HabitSchedule.IsActive,
                         NextDueUtc = h.HabitSchedule.NextDueUtc
                     } : null,
                 }).ToListAsync();

        }

        public async Task<GetHabitDto?> GetHabitByIdAsync(int id, int UserId)
        {
            return await appDbContext.Habits.AsNoTracking().Where(h => h.Id == id && h.UserId == UserId).Select(h => new GetHabitDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                TargetPeriod = h.TargetPeriod,
                CreatedAt = h.CreatedAt,
                HabitSchedule = h.HabitSchedule != null ? new GetHabitScheduleDto
                {
                    WeeklyDaysMask = h.HabitSchedule.WeeklyDaysMask,
                    TimeZoneId = h.HabitSchedule.TimeZoneId,
                    PreferredLocalTime = h.HabitSchedule.PreferredLocalTime,
                    IsActive = h.HabitSchedule.IsActive,
                    NextDueUtc = h.HabitSchedule.NextDueUtc
                } : null,
            }).FirstOrDefaultAsync();
        }

        public async Task<GetHabitDto?> GetHabitByName(string name, int UserId)
        {
            return await appDbContext.Habits.AsNoTracking().Where(h => h.Name == name && h.UserId == UserId).Select(h => new GetHabitDto
            {
                Id = h.Id,
                Name = h.Name,
                Description = h.Description,
                TargetPeriod = h.TargetPeriod,
                CreatedAt = h.CreatedAt,
                HabitSchedule = h.HabitSchedule != null ? new GetHabitScheduleDto
                {
                    WeeklyDaysMask = h.HabitSchedule.WeeklyDaysMask,
                    TimeZoneId = h.HabitSchedule.TimeZoneId,
                    PreferredLocalTime = h.HabitSchedule.PreferredLocalTime,
                    IsActive = h.HabitSchedule.IsActive,
                    NextDueUtc = h.HabitSchedule.NextDueUtc
                } : null,
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateHabitAsync(int id, UpdateHabitDto habitDto)
        {
            var habitExisting = await appDbContext.Habits.FindAsync(id);
            if (habitExisting == null) return false;

            if (String.IsNullOrWhiteSpace(habitExisting.Name)) throw new ArgumentException("Habit not found");

            habitExisting.Name = habitDto.Name.Trim();
            habitExisting.Description = habitDto.Description?.Trim();
            habitExisting.Periodicity = habitDto.Periodicity;
            habitExisting.TargetPeriod = habitDto.TargetPeriod;
            habitExisting.IsArchived = habitDto.IsArchived;
            await appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
