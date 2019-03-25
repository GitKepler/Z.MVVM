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
    ///     Null checking validation rule
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class NotNullValidationAttribute : BaseValidationAttribute
    {
        /// <inheritdoc />
        public NotNullValidationAttribute([NotNull] string propertyName) : base(propertyName) {
            ValueValidator.ArgumentNull(propertyName, nameof(propertyName));
            ErrorGenerator = o => o is null ? $"The value of {propertyName} is null" : string.Empty;
        }

        #region Overrides of BaseValidationAttribute

        /// <inheritdoc />
        public override Func<object, string> ErrorGenerator { get; }

        #endregion
    }
}