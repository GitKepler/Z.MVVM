#region USINGS

using System;
using System.Globalization;
using System.Windows.Data;


#endregion

namespace Z.MVVMHelper.Converters
{
    /// <inheritdoc />
    /// <summary>
    /// Perform an inversion of a <see cref="bool"/> value
    /// </summary>
    public class InverterConverter : IValueConverter
    {
        /// <inheritdoc />
        /// <summary>
        /// Convert the value to its opposite
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture) {
            if (value is bool b) {
                return !b;
            }

            throw new ArgumentException($"The provided value is not of the correct type.", nameof(value));
        }

        /// <inheritdoc />
        /// <summary>
        /// Convert back the value to the original
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return Convert(value, targetType, parameter, culture);
        }
    }
}