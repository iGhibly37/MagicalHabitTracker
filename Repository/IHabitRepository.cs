using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Repository
{
    public interface IHabitRepository
    {
        Task<List<Habit>> GettAllHabitsAsync();
        Task<Habit?> GetHabitByIdAsync(int id);
        Task<Habit> GetHabitByName(String name);
        Task AddSync(Habit habit);
        Task UpdateAsync(Habit habit);
        Task DeleteAsync(Habit habit);
    }
}
