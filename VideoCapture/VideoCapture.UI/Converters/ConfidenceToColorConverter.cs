// <copyright file="ConfidenceToColorConverter.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace VideoCapture.UI.Converters
{
    /// <summary>
    /// Convert to provide a suitable color based on the confidence.
    /// </summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    public class ConfidenceToColorConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // based on https://stackoverflow.com/questions/25007/conditional-formatting-percentage-to-color-conversion
            double percent = (double)value * 100;

            double red = (percent < 50) ? 255 : 256 - ((percent - 50) * 5.12);
            double green = (percent > 50) ? 255 : percent * 5.12;

            return new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)red, (byte)green, 0));
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
