using System.ComponentModel.DataAnnotations;

namespace StudentReservations.Date.Tables;

public class Group
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int ReservationCount { get; set; } = default!;
    public List<Reservation> Reservations { get; set; } = new();
}
