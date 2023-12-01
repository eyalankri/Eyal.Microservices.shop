using Members.Entities;

namespace Members;

public class UserList
{
    public static List<User> Users = new List<User>()
    {
        new User { Email = "avi.levi@email.com", Password = "Avi!.df%j#", Role = "Seller", Name="Avi", Surname="Levi"},
        new User { Email = "eyal.ank@gmail.com", Password = "123qwe", Role = "Administrator", Name="Eyal", Surname="Ankri" },
    };
}