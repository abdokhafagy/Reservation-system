using Microsoft.EntityFrameworkCore;
using StudentReservations.Date.Tables;

namespace StudentReservations.Date;

public class AppDbContext  :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Group> Groups { get; set; }
}
