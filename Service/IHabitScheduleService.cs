using MagicalHabitTracker.Dto.ScheduleDtos;
using MagicalHabitTracker.Model;

namespace MagicalHabitTracker.Service
{
    public interface IHabitScheduleService
    {
        public Task<List<GetHabitScheduleDto>> GetAllSchedulesAsync();
        public Task<GetHabitScheduleDto?> GetScheduleByIdAsync(int id);
        public Task<int> CreateScheduleAsync(int id,CreateHabitScheduleDto habitSchedDto);
        public Task<bool> PutScheduleAsync(int id, UpdateHabitScheduleDto habitSchedDto);
        public Task<bool> PatchScheduleAsync(int id, PatchScheduleDto habitSchedDto);
        public Task<bool> DeleteScheduleAsync(int id);
    }
}
