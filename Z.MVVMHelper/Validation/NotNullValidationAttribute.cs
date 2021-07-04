#region USINGS

using System;

#endregion

namespace Z.MVVMHelper.Validation
{
    /// <inheritdoc />
    /// <summary>
    ///     Null checking validation rule
    /// </summary>
    public sealed class NotNullValidationAttribute : BaseValidationAttribute
    {
        /// <inheritdoc />
        public NotNullValidationAttribute(string propertyName) : base(propertyName) {
            if (propertyName is null) throw new ArgumentNullException(nameof(propertyName));

            ErrorGenerator = o => o is null ? $"The value of {propertyName} is null." : string.Empty;
        }

        #region Overrides of BaseValidationAttribute

        /// <inheritdoc />
        public override Func<object?, string> ErrorGenerator { get; }

        #endregion
    }
}