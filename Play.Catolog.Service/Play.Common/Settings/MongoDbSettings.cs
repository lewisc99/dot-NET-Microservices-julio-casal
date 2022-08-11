

namespace Play.Common.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; init; }
        public int Port { get; init; } //to not modify the value after initialized it.
        public string ConnectionString => $"mongodb://{Host}:{Port}";

    }
}
