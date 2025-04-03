using System.Reflection;

namespace StickedWords.Application;

internal static class AssemblyReference
{
    public readonly static Assembly Assembly = typeof(AssemblyReference).Assembly;
}
