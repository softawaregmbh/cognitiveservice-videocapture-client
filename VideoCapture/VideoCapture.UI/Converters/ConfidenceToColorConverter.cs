using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace VideoCapture.UI.Converters
{
    public class ConfidenceToColorConverter : IValueConverter
    {
        // based on https://stackoverflow.com/questions/25007/conditional-formatting-percentage-to-color-conversion
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double percent = (double)value*100;

            double red = (percent < 50) ? 255 : 256 - (percent - 50) * 5.12;
            double green = (percent > 50) ? 255 : percent * 5.12;
            
            return new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)red, (byte)green, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
