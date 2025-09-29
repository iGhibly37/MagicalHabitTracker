using MagicalHabitTracker.Data;
using MagicalHabitTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace MagicalHabitTracker.Repository
{
    public class HabitRepository : IHabitRepository
    {
        private readonly AppDbContext _context;

        public HabitRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddSync(Habit habit)
        {
            _context.Habits.Add(habit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Habit habit)
        {
            _context.Habits.Remove(habit);
            await _context.SaveChangesAsync();
        }

        public async Task<Habit?> GetHabitByIdAsync(int id)
        {
            return await _context.Habits.FindAsync(id).AsTask();
        }

        public async Task<Habit> GetHabitByName(string name)
        {
            return await _context.Habits.FirstOrDefaultAsync(h => h.Name == name);
        }

        public async Task<List<Habit>> GettAllHabitsAsync()
        {
            return await _context.Habits.ToListAsync();
        }

        public Task UpdateAsync(Habit habit)
        {
            _context.Habits.Update(habit);
            return _context.SaveChangesAsync();
        }
    }
}
