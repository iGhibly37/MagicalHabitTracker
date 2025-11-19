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
using MagicalHabitTracker.Dto.ScheduleDtos;

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
        public async Task<ActionResult<IEnumerable<GetHabitScheduleDto>>> GetSchedules()
        {
            var schedules = await _habitScheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        // GET: api/HabitSchedules/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetHabitScheduleDto>> GetHabitSchedule(int id)
        {
            var schedule = await _habitScheduleService.GetScheduleByIdAsync(id);

            if(schedule == null) return NotFound();

            return Ok(schedule);
            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutHabitSchedule(int id, UpdateHabitScheduleDto dto)
        {
            bool updated = await _habitScheduleService.PutScheduleAsync(id, dto);
            if(!updated) return NotFound();
            return NoContent();
        }

        [HttpPost("{habitId:int}/schedule")]
        public async Task<ActionResult<CreateHabitScheduleDto>> PostHabitSchedule(int habitId, [FromBody] CreateHabitScheduleDto dto)
        {
            var habitScheduleId = await _habitScheduleService.CreateScheduleAsync(habitId, dto);

            return CreatedAtAction(nameof(GetHabitSchedule), new { id = habitScheduleId }, dto);
        }

        [HttpPatch("{habitId:int}/patchSchedule")]
        public async Task<ActionResult<PatchScheduleDto>> PatchHabitSchedule(int habitId, PatchScheduleDto dto)
        {
            bool updated = await _habitScheduleService.PatchScheduleAsync(habitId, dto);
            if(!updated) return NotFound();
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHabitSchedule(int id)
        {
          var habit = await _habitScheduleService.DeleteScheduleAsync(id);
            if(!habit) return NotFound();
            return NoContent();
        }

    }
}
