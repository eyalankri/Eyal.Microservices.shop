using Members.Entities;

namespace Members;

public static class Extensions
{
    public static UserDto AsDto(this User user)
    {

        return new UserDto(user.Id, "***", user.Name != null ? user.Name : "", user.Role, user.Name!, user.Surname, user.DateCreated);
    }
}
