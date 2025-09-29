using MagicalHabitTracker.Dto;
using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Service
{
    public interface IHabitScheduleService
    {
        public Task<List<HabitScheduleDto>> GetAllSchedulesAsync();
        public Task<HabitScheduleDto?> GetScheduleByIdAsync(int id);
        public Task<int> CreateScheduleAsync(int id,HabitScheduleDto habitSchedDto);
        public Task<bool> UpdateScheduleAsync(int id, HabitScheduleDto habitSchedDto);
        public Task<bool> DeleteScheduleAsync(int id);
    }
}
