using Common;

namespace Members.Entities;

public class User : IEntity
{
    public User()
    {        
        DateCreated = DateTime.UtcNow;
    }

    public Guid Id { get; set; }

    public string Password { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTimeOffset DateCreated { get; set; }
}

