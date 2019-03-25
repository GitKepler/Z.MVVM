#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper.Validation
{
    /// <inheritdoc />
    /// <summary>
    ///     Check whether a <see cref="T:System.DateTime" /> is between two <see cref="T:System.DateTime" />
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class DateBetweenValidationAttribute : BaseValidationAttribute
    {
        /// <inheritdoc />
        [CLSCompliant(false)]
        public DateBetweenValidationAttribute([NotNull] string propertyName, DateTime lower, DateTime upper) : base(propertyName) {
            ValueValidator.ArgumentNull(propertyName, nameof(propertyName));
            ErrorGenerator = d => d is DateTime t
                ? t < lower
                    ? $"{t} is not after {lower}."
                    : t > upper
                        ? $"{t} is not before {upper}."
                        : string.Empty
                : $"{d} is not a valid {typeof(DateTime)}.";
        }

        /// <inheritdoc />
        public DateBetweenValidationAttribute([NotNull] string propertyName, long lower, long upper) : this(propertyName, new DateTime(lower), new DateTime(upper)) { }

        #region Overrides of BaseValidationAttribute

        /// <inheritdoc />
        public override Func<object, string> ErrorGenerator { get; }

        #endregion
    }
}