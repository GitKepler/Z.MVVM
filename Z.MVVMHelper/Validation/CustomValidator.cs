#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using JetBrains.Annotations;
using Z.MVVMHelper.Interfaces;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper.Validation
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class CustomValidator<T> : IValidator
    {
        /// <inheritdoc />
        public CustomValidator([NotNull] string propertyName, [NotNull] Expression<Predicate<T>> filter) {
            ValueValidator.ArgumentNull(propertyName, nameof(propertyName));
            ValueValidator.ArgumentNull(filter, nameof(filter));
            PropertyName = propertyName;
            var predicateCode = filter.ToString();
            Predicate<T> predicate = filter.Compile();
            ErrorGenerator = o => o is T t
                ? predicate(t)
                    ? string.Empty
                    : $" {predicateCode} did not pass."
                : "Invalid type.";
        }

        /// <inheritdoc />
        public CustomValidator([NotNull] string propertyName, [NotNull] Predicate<T> filter, [NotNull] Func<T, string> errorGenerator) {
            ValueValidator.ArgumentNull(filter, nameof(filter));
            ValueValidator.ArgumentNull(propertyName, nameof(propertyName));
            ValueValidator.ArgumentNull(errorGenerator, nameof(errorGenerator));
            PropertyName = propertyName;
            Predicate<T> predicate = filter;
            ErrorGenerator = o => o is T t
                ? predicate(t)
                    ? string.Empty
                    : errorGenerator(t)
                : "Invalid type.";
        }

        #region Overrides of BaseValidationAttribute

        /// <inheritdoc />
        public Func<object, string> ErrorGenerator { get; }

        /// <inheritdoc />
        public string PropertyName { get; }

        #endregion
    }
}