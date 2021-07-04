using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace Z.MVVMHelper.Converters
{
    /// <summary>
    /// Convert a <see cref="bool"/> to a <see cref="Visibility"/> and back
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool b) {
                return b ? Visibility.Visible : Visibility.Hidden;
            }
            throw new ArgumentException($"The provided value is not of the correct type.", nameof(value));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Visibility v) {
                return v > 0;
            }
            throw new ArgumentException($"The provided value is not of the correct type.", nameof(value));
        }

        #endregion
    }
}
