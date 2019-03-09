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
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper.Validation
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [CLSCompliant(false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class CustomValidationAttribute : BaseValidationAttribute
    {
        /// <summary>
        ///     Code of the predicate
        /// </summary>
        [NotNull] public readonly string PredicateCode;

        /// <inheritdoc />
        public CustomValidationAttribute([NotNull] string propertyName, [NotNull] Expression<Predicate<object>> filter) : base(propertyName) {
            ValueValidator.ArgumentNull(filter, nameof(filter));
            PredicateCode = filter.ToString();
            Predicate<object> predicate = filter.Compile();
            ErrorGenerator = o => predicate(o) ? string.Empty : $" {PredicateCode} did not pass.";
        }

        /// <inheritdoc />
        public CustomValidationAttribute([NotNull] string propertyName, [NotNull] Expression<Predicate<object>> filter, [NotNull] Func<object, string> errorGenerator) : base(propertyName) {
            ValueValidator.ArgumentNull(filter, nameof(filter));
            ValueValidator.ArgumentNull(errorGenerator, nameof(errorGenerator));
            PredicateCode = filter.ToString();
            Predicate<object> predicate = filter.Compile();
            ErrorGenerator = o => predicate(o) ? string.Empty : errorGenerator(o);
        }

        #region Overrides of BaseValidationAttribute

        /// <inheritdoc />
        public override Func<object, string> ErrorGenerator { get; }

        #endregion
    }
}