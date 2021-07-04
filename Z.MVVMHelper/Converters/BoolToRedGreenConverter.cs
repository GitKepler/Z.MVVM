#region USINGS

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace Z.MVVMHelper.Converters
{
    /// <inheritdoc />
    /// <summary>
    ///     Convert a <see cref="T:System.Boolean" /> to <see cref="P:System.Windows.Media.Brushes.Green" /> if true or
    ///     <see cref="P:System.Windows.Media.Brushes.Red" /> if false
    /// </summary>
    public class BoolToRedGreenConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool b) {
                return b ? Brushes.Green : Brushes.Red;
            }

            throw new ArgumentException($"The provided value is not of the correct type.", nameof(value));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Brush b) {
                return b == Brushes.Green;
            }

            throw new ArgumentException($"The provided value is not of the correct type.", nameof(value));
        }

        #endregion
    }
}