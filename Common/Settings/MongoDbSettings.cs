namespace Common.Settings
{
    // we created a section with this name in appsettings.json
    public class MongoDbSettings
    {
        public string? Host { get; init; }
        public int Port { get; init; }
        public string ConnectionString => $"mongodb://{Host}:{Port}"; // => function returns $"mongodb://{Host}:{Port}";
    }
}
