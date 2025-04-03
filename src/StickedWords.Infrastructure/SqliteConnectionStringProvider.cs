using Microsoft.Extensions.Configuration;

namespace StickedWords.Infrastructure;

public static class SqliteConnectionStringProvider
{
    public static string? Get(IConfiguration configuration, string dbContextName)
    {
        var connectionString = configuration.GetConnectionString(dbContextName);

        return SetDataDirectory(connectionString);
    }

    private static string? SetDataDirectory(string? connectionString)
    {
        var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        return connectionString?.Replace("|DataDirectory|", basePath + Path.DirectorySeparatorChar);
    }
}
