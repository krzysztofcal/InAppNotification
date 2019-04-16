using System;
using System.Diagnostics;
using Windows.UI.Xaml.Data;

namespace InAppNotification.Converters
{
    public class DoubleToNegativeDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is double)) throw new InvalidOperationException();
            if (parameter != null)
            {
                if (!(parameter is double)) throw new InvalidOperationException();
                Debug.WriteLine($"DoubleToNegativeDoubleConverter: value = {(double)value}" +
                    $", offset = {(double)parameter},  result = {-(((double)value) + ((double)parameter))}");
                return -(((double)value) + ((double)parameter));
            }
            Debug.WriteLine($"DoubleToNegativeDoubleConverter: value = {(double)value}, result = {-(double)value}");
            return -(double)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}