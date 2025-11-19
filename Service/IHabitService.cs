using MagicalHabitTracker.Dto.HabitDtos;
using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Service
{
    public interface IHabitService
    {
        public Task<List<GetHabitDto>> GetAllHabitsAsync(int UserId);
        public Task<GetHabitDto?> GetHabitByIdAsync(int id, int UserId);
        public Task<GetHabitDto?> GetHabitByName(String name, int UserId);
        public Task<int> AddHabitAsync(CreateHabitDto habit, int userId);
        public Task<bool> UpdateHabitAsync(int id, UpdateHabitDto habit);
        public Task<bool> DeleteHabitAsync(int id);
    }
}
