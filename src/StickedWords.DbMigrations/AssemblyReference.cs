using System.Reflection;

namespace StickedWords.DbMigrations;

internal static class AssemblyReference
{
    public readonly static Assembly Assembly = typeof(AssemblyReference).Assembly;
}
