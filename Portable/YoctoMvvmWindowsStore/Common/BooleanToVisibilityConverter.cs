﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace YoctoMvvmWindowsStore.Common {
    public class BooleanToVisibilityConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (value is bool && (bool)value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException("Only implementing Convert");
        }
    }
}
