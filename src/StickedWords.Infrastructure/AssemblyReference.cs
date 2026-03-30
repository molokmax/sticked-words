using System.Reflection;

namespace StickedWords.Infrastructure;

internal static class AssemblyReference
{
    public readonly static Assembly Assembly = typeof(AssemblyReference).Assembly;
}
