// Developed for 3rdRock by Jim Barber (January 6, 2026)

using System.Globalization;
using System.Windows.Data;

namespace SSDToolsWPF.UI;

public class InverseBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return !boolValue;

        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return !boolValue;

        return true;
    }
}