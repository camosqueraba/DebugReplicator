﻿using DebugReplicator.Helpers;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DebugReplicator.Controller.Converters
{
    /// <summary>
    /// Convers <see cref="System.Drawing.Icon"/> to a <see cref="System.Windows.Media.ImageSource"/>
    /// </summary>
    public class IconToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Icon ico)
            {
                ImageSource img = ico.ToImageSource();
                ico.Dispose();
                return img;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
