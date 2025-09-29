using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MagicalHabitTracker.Data;
using MagicalHabitTracker.Model;
using MagicalHabitTracker.Service;
using MagicalHabitTracker.Dto;

namespace MagicalHabitTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitSchedulesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly HabitScheduleService _habitScheduleService;
        public HabitSchedulesController(AppDbContext context, HabitScheduleService habitScheduleService)
        {
            _context = context;
            _habitScheduleService = habitScheduleService;   
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<HabitScheduleDto>>> GetSchedules()
        {
            var schedules = await _habitScheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        // GET: api/HabitSchedules/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<HabitScheduleDto>> GetHabitSchedule(int id)
        {
            var schedule = await _habitScheduleService.GetScheduleByIdAsync(id);
            if(schedule == null) return NotFound();

            return Ok(schedule);
            
        }

        // PUT: api/HabitSchedules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutHabitSchedule(int id, HabitScheduleDto dto)
        {
            bool updated = await _habitScheduleService.UpdateScheduleAsync(id, dto);
            if(!updated) return NotFound();
            return NoContent();
        }

        // POST: api/HabitSchedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id:int}")]
        public async Task<ActionResult<HabitScheduleDto>> PostHabitSchedule(int habitId, HabitScheduleDto dto)
        {
            var Id = await _habitScheduleService.CreateScheduleAsync(habitId, dto);

            return CreatedAtAction(nameof(GetHabitSchedule), new { id = Id }, dto);
        }

        // DELETE: api/HabitSchedules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabitSchedule(int id)
        {
          var habit = await _habitScheduleService.DeleteScheduleAsync(id);
            if(!habit) return NotFound();
            return NoContent();
        }

        //private bool HabitScheduleExists(int id)
        //{
        //    return _context.Schedules.Any(e => e.Id == id);
        //}
    }
}
