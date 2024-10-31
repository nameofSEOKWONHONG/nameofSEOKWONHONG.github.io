using System.Globalization;

namespace MudExample.Infrastructure;

public static class DoubleExtensions
{
    public static string AsString(this double value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }
}