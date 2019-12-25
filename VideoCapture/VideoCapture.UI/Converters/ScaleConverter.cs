using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace VideoCapture.UI.Converters
{
    public class ScaleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
            {
                throw new ArgumentException("Please provide the original value and the scale factor as parameters.");
            }

            var originalValue = System.Convert.ToDouble(values[0]);
            var scaleFactor = System.Convert.ToDouble(values[1]);

            return originalValue * scaleFactor;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
