using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using JetBrains.Annotations;

namespace Z.MVVMHelper.Converters
{
    /// <summary>
    /// Convert a <see cref="bool"/> to a <see cref="Visibility"/> and back
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class BoolToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        [NotNull]
        public object Convert(object value, [CanBeNull] Type targetType, object parameter, [CanBeNull] CultureInfo culture) {
            if (value is bool b) {
                return b ? Visibility.Visible : Visibility.Hidden;
            }

            throw Internals.ExceptionGenerator.InvalidArgumentType<bool>(value?.GetType(), nameof(value));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, [CanBeNull] Type targetType, object parameter, [CanBeNull] CultureInfo culture) {
            if (value is Visibility v) {
                return v > 0;
            }

            throw Internals.ExceptionGenerator.InvalidArgumentType<Visibility>(value?.GetType(), nameof(value));
        }

        #endregion
    }
}
