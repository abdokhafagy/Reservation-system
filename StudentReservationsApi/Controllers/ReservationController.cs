using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentReservations.Date;
using StudentReservations.Date.Tables;
using StudentReservations.Models;

namespace StudentReservations.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationController : ControllerBase
{
    AppDbContext _dbContext;

    public ReservationController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public ActionResult<List<ReservationModel>> Get()
    {
        return _dbContext.Reservations.Select(g=>new ReservationModel
        {
            Name = g.Name,
            Address = g.Address,
            Email = g.Email,
            Group = g.Group.Name,
            Phone = g.Phone,
            PhoneParent1 = g.PhoneParent1,  
            PhoneParent2 = g.PhoneParent2,
        }).AsNoTracking().ToList();
    }

    [HttpGet("/{groupName}")]
    public async Task<ActionResult<List<Reservation>>> GetByGroupName(string groupName)
    {
        return await _dbContext.Reservations.AsNoTracking().Where(r=>r.Group.Name==groupName).ToListAsync();
    }

    [HttpGet("{id}")]
    public ActionResult<ReservationModel> GetById(int id)
    {
        var reservation = _dbContext.Reservations.Find(id);
        
        return  new ReservationModel()
        {
            Name = reservation.Name,
            Address = reservation.Address,
            Email = reservation.Email,
            GroupId = reservation.GroupId,
            Phone = reservation.Phone,
            PhoneParent1 = reservation.PhoneParent1,
            PhoneParent2 = reservation.PhoneParent2,
            
        };
    }

    [HttpGet("resingroup/{id?}")]
    public async Task<ActionResult<IEnumerable<ReservationModel>>> GetAllReservationsByGroupId(int? id)
    {
        IQueryable<Reservation> query = _dbContext.Reservations;

        if (id.HasValue)
        {
            query = query.Where(r => r.GroupId == id.Value);
        }

        var reservations = await query
            .Select(r => new ReservationModel
            {
                Name = r.Name,
                Address = r.Address,
                Email = r.Email,
                GroupId = r.GroupId,
                Phone = r.Phone,
                PhoneParent1 = r.PhoneParent1,
                PhoneParent2 = r.PhoneParent2,
                Group = r.Group.Name
            })
            .ToListAsync();

        return Ok(reservations);
    }

    [HttpGet("Count/{id}")]
    public async Task<ActionResult> GetAllReservationsCountByGroupId(int id)
    {
        

        var reservations = await _dbContext.Reservations
            .Where(r => r.GroupId == id)
            .CountAsync();

        return Ok(reservations);
    }
    [HttpPost()]
    public async Task<ActionResult<ReservationModel>> Create(ReservationModel model)
    {
        // Count existing reservations for the given group
        //int groupReservationCount = await _dbContext.Reservations
        //    .CountAsync(r => r.Group == model.Group);

        var resCount = await _dbContext.Groups
            .Where(g => g.Id == model.GroupId)
            .Select(g => g.ReservationCount)
            .FirstOrDefaultAsync();

        if (await _dbContext.Reservations.CountAsync(r=>r.GroupId == model.GroupId) >= resCount)
        {
            return BadRequest($"لا يمكن الاضافة لهذا الجروب");
        }
        if (await _dbContext.Reservations.AnyAsync(r=>r.Phone == model.Phone) ||
           (await _dbContext.Reservations.AnyAsync(r => r.Email == model.Email && r.Email == null)))
        {
            return BadRequest(new { message = " تم التسجيل من قبل " });
        }

        Reservation reservation=  new()
        {
            Name = model.Name,
            Address = model.Address,
            Email = model.Email,
            GroupId = model.GroupId,
            Phone = model.Phone,
            PhoneParent1 = model.PhoneParent1,
            PhoneParent2 = model.PhoneParent2,
            
        };
        _dbContext.Reservations.Add(reservation);
        await _dbContext.SaveChangesAsync();  
  
        return CreatedAtAction("Get", new { id = reservation.Id }, reservation);  
    }
    [HttpPut("/{id}")]
    public async Task<ActionResult<ReservationModel>> Update(int id,ReservationModel model)
    {
        var UpdateRes = await _dbContext.Reservations.FindAsync(id);
        UpdateRes.Name = model.Name;
        UpdateRes.Address = model.Address;
        UpdateRes.Email = model.Email;
        UpdateRes.Phone = model.Phone;
        UpdateRes.PhoneParent2 = model.PhoneParent2;
        UpdateRes.PhoneParent1 = model.PhoneParent1;
        UpdateRes.GroupId = model.GroupId;
        
        
        _dbContext.Reservations.Update(UpdateRes);
        await _dbContext.SaveChangesAsync();  
  
        return CreatedAtAction("Get", new { id = UpdateRes.Id }, UpdateRes);  
    }

    //[HttpGet("/group-status")]
    //public async Task<IActionResult> GetGroupStatuses()
    //{
    //    var groups = await _dbContext.Groups
            
    //        .Select(g => new
    //        {
    //            g.Name,
    //            g.Id,
    //            g.ReservationCount
    //        })
    //        .ToListAsync();

    //    return Ok(groups);
    //}
}
