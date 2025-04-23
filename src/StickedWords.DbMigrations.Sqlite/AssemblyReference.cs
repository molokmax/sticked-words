using System.Reflection;

namespace StickedWords.DbMigrations.Sqlite;

public static class AssemblyReference
{
    public readonly static Assembly Assembly = typeof(AssemblyReference).Assembly;
}
