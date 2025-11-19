using MagicalHabitTracker.Dto.HabitTrackerDtos;

namespace MagicalHabitTracker.Service
{
    public interface ITrackerService
    {
        public Task<List<HabitTrackerDto>> GetAll();
        public Task<HabitTrackerDto> GetTrackerAsync(int id);
        //public Task<bool> UpdateTrackerAsync(int id, UpdateHabitTrackerDto dto);
        public Task<bool> DeletTrackerAsync(int id);
        public Task<int> CreateTrackerAsync(int id, CreateHabitTrackerDto dto);
        public Task<bool> MarkHabitCompletedAsync(int id, DateOnly date);
        public Task<List<HabitTrackerDto>> GetHistoryByHabitId(int habitId);

    }
}
