using System.Reflection;

namespace StickedWords.DbMigrations.Postgres;

public static class AssemblyReference
{
    public readonly static Assembly Assembly = typeof(AssemblyReference).Assembly;
}
