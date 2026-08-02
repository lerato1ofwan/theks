namespace Theks.Identity.Domain.Entities;

public class ApplicationUser
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string ContactNumber { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string Address { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}