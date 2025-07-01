using System.ComponentModel.DataAnnotations;

namespace StudentReservations.Date.Tables;

public class Reservation
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string PhoneParent1 { get; set; } = default!;
    public string? PhoneParent2 { get; set; } 
    public string Address { get; set; } = default!;
    public string? Email { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; } = default!;
}
