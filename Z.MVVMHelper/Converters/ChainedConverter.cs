#region USINGS

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;


#endregion

namespace Z.MVVMHelper.Converters
{
    /// <summary>
    ///     Chain multiple <see cref="T:System.Windows.Data.IValueConverter" /> together
    /// </summary>
    public class ChainedConverter : List<IValueConverter>, IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return this
                .Where(c => !(c is null))
                .Aggregate(value, (b, n) => n.Convert(b, targetType, parameter, culture));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return this
                .Where(c => !(c is null))
                .Reverse()
                .Aggregate(value, (b, n) => n.ConvertBack(b, targetType, parameter, culture));
        }

        #endregion Implementation of IValueConverter
    }
}