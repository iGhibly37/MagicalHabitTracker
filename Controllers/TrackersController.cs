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
using MagicalHabitTracker.Dto.HabitTrackerDtos;

namespace MagicalHabitTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TrackerService _trackerService;

        public TrackersController(AppDbContext context, TrackerService trackerService)
        {
            _context = context;
            _trackerService = trackerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HabitTrackerDto>>> GetHabitTrackers()
        {
            return Ok(await _trackerService.GetAll());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<HabitTrackerDto>> GetTrackerByIdAsync(int id)
        {
            var tracker = await _trackerService.GetTrackerAsync(id);

            if (tracker == null)
            {
                return NotFound();
            }

            return Ok(tracker);
        }

        [HttpPost("{habitId:int}")]
        public async Task<ActionResult<HabitTrackerDto>> PostTracker(int habitId, CreateHabitTrackerDto dto)
        {
            int Id = await _trackerService.CreateTrackerAsync(habitId, dto);
            return CreatedAtAction(nameof(GetTrackerByIdAsync), new {id = Id}, dto);
        }

        // DELETE: api/Trackers/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTracker(int id)
        {
            bool deleted = await _trackerService.DeletTrackerAsync(id);
            if(!deleted) return NotFound();
            return NoContent();
        }

        private bool TrackerExists(int id)
        {
            return _context.HabitTrackers.Any(e => e.Id == id);
        }
    }
}
