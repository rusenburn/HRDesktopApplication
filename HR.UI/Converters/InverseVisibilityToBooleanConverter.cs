using System.Globalization;
using System.Windows;

namespace HR.UI.Converters;
public class InverseVisibilityToBooleanConverter : BaseValueConverter<InverseVisibilityToBooleanConverter>
{
    public static InverseVisibilityToBooleanConverter Instance = new InverseVisibilityToBooleanConverter();

    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is bool boolValue && boolValue ? Visibility.Collapsed : Visibility.Visible;
    }
    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
