namespace StudentReservations.Models;

public class ReservationModel
{
    public string Name { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string PhoneParent1 { get; set; } = default!;
    public string? PhoneParent2 { get; set; }
    public string Address { get; set; } = default!;
    public string? Email { get; set; }
    public string Group { get; set; } = default!;
    public int GroupId { get; set; } = default!;
}
