using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentReservations.Date;
using StudentReservations.Date.Tables;
using StudentReservations.Models;
using StudentReservations.Models.GroupModel;

namespace StudentReservations.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        AppDbContext _dbContext;

        public GroupsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("/group-status")]
        public async Task<IActionResult> GetGroupStatuses()
        {
            var groups = await _dbContext.Groups

                .Select(g => new
                {
                    g.Name,
                    g.Id,
                    g.ReservationCount
                })
                .ToListAsync();

            return Ok(groups);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllAsync()
        {
            var groups = await _dbContext.Groups.Include(g=>g.Reservations)
                .ToListAsync();

            return Ok(groups);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync(GroupModel model)
        {
            // validation on group name 
            if (await _dbContext.Groups.AnyAsync(g => g.Name == model.Name))
                return BadRequest("هذا الجروب موجود من قبل ");

            Group group = new()
            {
                Name = model.Name,
                 ReservationCount = model.ReservationCount

            };
            _dbContext.Groups.Add(group);
            await _dbContext.SaveChangesAsync();

            return Ok( group);
        }

        [HttpPut("/group/{id}")]
        public async Task<ActionResult<GroupModel>> Update(int id, GroupModel model)
        {
            var UpdateGroup = await _dbContext.Groups.FindAsync(id);
            UpdateGroup.Name = model.Name;
            UpdateGroup.ReservationCount = model.ReservationCount;



            _dbContext.Groups.Update(UpdateGroup);
            await _dbContext.SaveChangesAsync();

            return Ok( UpdateGroup);
        }
    }
}
