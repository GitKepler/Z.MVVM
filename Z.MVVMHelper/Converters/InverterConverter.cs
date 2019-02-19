#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using JetBrains.Annotations;

#endregion

namespace Z.MVVMHelper.Converters
{
    /// <inheritdoc />
    /// <summary>
    /// Perform an inversion of a <see cref="bool"/> value
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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
        public object Convert(object value, [CanBeNull] Type targetType, object parameter,
            [CanBeNull] CultureInfo culture) {
            if (value is bool b) {
                return !b;
            }

            throw Internals.ExceptionGenerator.InvalidArgumentType<bool>(value?.GetType(), nameof(value));
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
        public object ConvertBack(object value, [CanBeNull] Type targetType, object parameter, [CanBeNull] CultureInfo culture) {
            return Convert(value, targetType, parameter, culture);
        }
    }
}