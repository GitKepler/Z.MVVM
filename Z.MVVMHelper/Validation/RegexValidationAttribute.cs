#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Interfaces;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper.Validation
{
    /// <inheritdoc cref="Attribute" />
    /// <summary>
    ///     Regex validation rule
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class RegexValidationAttribute : BaseValidationAttribute
    {
        [NotNull] private readonly Regex _regex;

        /// <inheritdoc />
        /// <summary>
        ///     Create a regex validation rule with a custom error generator
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="regex">Regex to match</param>
        /// <param name="errorGenerator">Error generator</param>
        public RegexValidationAttribute([NotNull] string propertyName, [NotNull] string regex, [NotNull] Func<string, string> errorGenerator) : base(propertyName) {
            ValueValidator.ArgumentNull(regex, nameof(regex));
            ValueValidator.ArgumentNull(errorGenerator, nameof(errorGenerator));
            _regex = new Regex(regex, RegexOptions.Compiled | RegexOptions.CultureInvariant);
            ErrorGenerator = ValidateRegex(errorGenerator, propertyName);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Create a regex validation rule
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="regex"></param>
        public RegexValidationAttribute([NotNull] string propertyName, [NotNull] string regex) : base(propertyName) {
            ValueValidator.ArgumentNull(regex, nameof(regex));
            _regex = new Regex(regex, RegexOptions.Compiled | RegexOptions.CultureInvariant);
            ErrorGenerator = ValidateRegex(s => $"\"{s}\" is not a valid value for {propertyName}", propertyName);
        }

        #region Overrides of BaseValidationAttribute

        /// <inheritdoc />
        public override Func<object, string> ErrorGenerator { get; }

        #endregion

        [NotNull]
        private Func<object, string> ValidateRegex([NotNull] Func<string, string> errorGenerator, [NotNull] string propertyName) {
            ValueValidator.ArgumentNull(propertyName, nameof(propertyName));
            ValueValidator.ArgumentNull(errorGenerator, nameof(errorGenerator));
            return o =>
            {
                var prep = PreprocessValue<string>(o);
                if (prep is null) {
                    var s = (string) o;
                    return _regex.IsMatch(s ?? string.Empty) ? string.Empty : errorGenerator(s);
                }

                return prep;
            };
        }
    }
}