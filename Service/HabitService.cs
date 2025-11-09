using MagicalHabitTracker.Data;
using MagicalHabitTracker.Dto;
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

        public async Task<int> AddHabitAsync(HabitEditDto habitDto, int userId)
        {
            if(String.IsNullOrWhiteSpace(habitDto.Name))
            {
                throw new ArgumentException("Habit name cannot be null or empty.");
            }

            if(String.IsNullOrWhiteSpace(habitDto.Name.Trim()))
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
                IsArchived = habitDto.IsArchived,
                CreatedAt = DateTime.UtcNow
            };
            appDbContext.Habits.Add(habit);
            await appDbContext.SaveChangesAsync();
            return habit.Id;
        }

        public async Task<bool> DeleteHabitAsync(int id)
        {
            var habit = await appDbContext.Habits.FindAsync(id);
            if(habit == null || habit.Id <= 0)
                throw new ArgumentException("Invalid habit.");


            appDbContext.Habits.Remove(habit);
            await appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Habit>> GetAllHabitsAsync()
        {
           return await appDbContext.Habits.OrderBy(h => h.IsArchived)
                .ThenBy(h => h.Name)
                .AsNoTracking()
                .ToListAsync();

        }

        public async Task<Habit?> GetHabitByIdAsync(int id){
            return await appDbContext.Habits.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);

        }

        public async Task<Habit?> GetHabitByName(string name)
        {
           return await appDbContext.Habits.AsNoTracking().FirstOrDefaultAsync(h => h.Name == name);
        }

        public async Task<bool> UpdateHabitAsync(int id, HabitEditDto habitDto)
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
