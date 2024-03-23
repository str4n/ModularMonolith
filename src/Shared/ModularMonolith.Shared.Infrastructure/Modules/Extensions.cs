namespace ModularMonolith.Shared.Infrastructure.Modules;

internal static class Extensions
{
    private const string NamespacePart = "Modules";
    
    public static string GetModuleName(this object value)
    {
        if (value?.GetType() is null)
        {
            return string.Empty;
        }

        var type = value.GetType();
        
        if (type.Namespace is null)
        {
            return string.Empty;
        }

        return type.Namespace.Contains(NamespacePart) ? type.Namespace.Split(".")[2] : string.Empty;
    }
}