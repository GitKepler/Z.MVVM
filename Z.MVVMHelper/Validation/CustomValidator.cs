#region USINGS

using System;
using System.Linq.Expressions;

using Z.MVVMHelper.Interfaces;

#endregion

namespace Z.MVVMHelper.Validation
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public sealed class CustomValidator<T> : IValidator
    {
        /// <inheritdoc />
        public CustomValidator(string propertyName, Expression<Predicate<T>> filter) {
            if (propertyName is null) throw new ArgumentNullException(nameof(propertyName));
            if (filter is null) throw new ArgumentNullException(nameof(filter));

            PropertyName = propertyName;
            var predicateCode = filter.ToString();
            var predicate = filter.Compile();
            ErrorGenerator = o => o is T t
                ? predicate(t)
                    ? string.Empty
                    : $"{predicateCode} did not pass."
                : "Invalid type.";
        }

        /// <inheritdoc />
        public CustomValidator(string propertyName, Predicate<T> filter, Func<T, string> errorGenerator) {
            if (propertyName is null) throw new ArgumentNullException(nameof(propertyName));
            if (filter is null) throw new ArgumentNullException(nameof(filter));
            if (errorGenerator is null) throw new ArgumentNullException(nameof(errorGenerator));

            PropertyName = propertyName;
            var predicate = filter;
            ErrorGenerator = o => o is T t
                ? predicate(t)
                    ? string.Empty
                    : errorGenerator(t)
                : "Invalid type.";
        }

        #region Overrides of BaseValidationAttribute

        /// <inheritdoc />
        public Func<object?, string> ErrorGenerator { get; }

        /// <inheritdoc />
        public string PropertyName { get; }

        #endregion
    }
}