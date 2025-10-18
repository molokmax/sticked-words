namespace StickedWords.Domain.Exceptions;

public sealed class DbProviderNotSupportedException : Exception
{
    private static readonly string _message = "Db provider [{0}] is not supported";

    public DbProviderNotSupportedException(string dbProvider)
        : base(string.Format(_message, dbProvider))
    {
        DbProvider = dbProvider;
    }

    public string DbProvider { get; }
}
