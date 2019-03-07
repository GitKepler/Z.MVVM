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

#endregion USINGS

namespace Z.MVVMHelper.Converters
{
    /// <summary>
    ///     Chain multiple <see cref="T:System.Windows.Data.IValueConverter" /> together
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ChainedConverter : List<IValueConverter>, IValueConverter
    {
        #region Implementation of IValueConverter

        /// <inheritdoc />
        public object Convert(object value, [CanBeNull] Type targetType, object parameter,
            [CanBeNull] CultureInfo culture)
        {
            return this.Where(c => !(c is null))
                .Aggregate(value, (b, n) => n.Convert(b, targetType, parameter, culture));
        }

        /// <inheritdoc />
        public object ConvertBack(object value, [CanBeNull] Type targetType, object parameter,
            [CanBeNull] CultureInfo culture)
        {
            return this.AsEnumerable()
                ?.Where(c => !(c is null))
                .Reverse()
                .Aggregate(value, (b, n) => n.ConvertBack(b, targetType, parameter, culture));
        }

        #endregion Implementation of IValueConverter
    }
}