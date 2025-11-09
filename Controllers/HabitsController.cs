using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MagicalHabitTracker.Data;
using MagicalHabitTracker.Model;
using MagicalHabitTracker.Dto;
using MagicalHabitTracker.Service;
using System.Security.Claims;

namespace MagicalHabitTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHabitService _habitService;
       

        public HabitsController(AppDbContext context, IHabitService habitService)
        {
            _context = context;
            _habitService = habitService;
        }

        [HttpGet]
        public async Task<IActionResult> GeAll(){
           List<Habit> habits = await _habitService.GetAllHabitsAsync();
            return Ok(habits);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id){
            var habit = await _habitService.GetHabitByIdAsync(id);
            if (habit == null) return NotFound();
            return Ok(habit);
        }

        [HttpPost("registerHabit")]
        public async Task<IActionResult> Create(HabitEditDto habitDto)
        {
            var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(!int.TryParse(sub, out int userId))
            {
                return Unauthorized();
            }

            var Id = await _habitService.AddHabitAsync(habitDto, userId);

            return CreatedAtAction(nameof(GetById), new { Id = Id }, habitDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(int id, HabitEditDto habitDto)
        {
            var habit = await _habitService.UpdateHabitAsync(id, habitDto);
            if(!habit) return NotFound();
            return NoContent();
                
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var deleted = await _habitService.DeleteHabitAsync(id);
                return NoContent();
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
