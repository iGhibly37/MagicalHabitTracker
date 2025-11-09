using MagicalHabitTracker.Dto;
using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Service
{
    public interface IHabitService
    {
        Task<List<Habit>> GetAllHabitsAsync();
        Task<Habit?> GetHabitByIdAsync(int id);
        Task<Habit?> GetHabitByName(String name);
        Task<int> AddHabitAsync(HabitEditDto habit, int userId);
        Task<bool> UpdateHabitAsync(int id, HabitEditDto habit);
        Task<bool> DeleteHabitAsync(int id);
    }
}
