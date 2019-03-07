#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using JetBrains.Annotations;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper.Converters
{
    /// <inheritdoc />
    /// <summary>
    ///     Convert a <see cref="T:System.Boolean" /> to <see cref="P:System.Windows.Media.Brushes.Green" /> if true or
    ///     <see cref="P:System.Windows.Media.Brushes.Red" /> if false
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class BoolToRedGreenConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, [CanBeNull] Type targetType, object parameter, [CanBeNull] CultureInfo culture) {
            if (value is bool b) {
                return b ? Brushes.Green : Brushes.Red;
            }

            throw ExceptionGenerator.InvalidArgumentType<bool>(value?.GetType(), nameof(value));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, [CanBeNull] Type targetType, object parameter, [CanBeNull] CultureInfo culture) {
            if (value is Brush b) {
                return b == Brushes.Green;
            }

            throw ExceptionGenerator.InvalidArgumentType<Brush>(value?.GetType(), nameof(value));
        }

        #endregion
    }
}