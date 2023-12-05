namespace Members.Settings
{
    public class MembersApiSettings
    {
        public required string PasswordSalt { get; set; }
        public required string JwtToken { get; set; }
    }
}
