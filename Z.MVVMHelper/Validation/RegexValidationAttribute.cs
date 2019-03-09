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
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public sealed class RegexValidationAttribute : Attribute, IValidatorAttribute
    {
        /// <inheritdoc />
        /// <summary>
        ///     Create a regex validation rule with a custom error generator
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="regex">Regex to match</param>
        /// <param name="errorGenerator">Error generator</param>
        public RegexValidationAttribute([NotNull] string propertyName, [NotNull] string regex, [NotNull] Func<string, string> errorGenerator) {
            ValueValidator.ArgumentNull(regex, nameof(regex));
            ValueValidator.ArgumentNull(propertyName, nameof(propertyName));
            ValueValidator.ArgumentNull(errorGenerator, nameof(errorGenerator));
            var regex1 = new Regex(regex, RegexOptions.Compiled | RegexOptions.CultureInvariant);
            ErrorGenerator = ValidateRegex(errorGenerator, regex1, propertyName);
            PropertyName = propertyName;
        }

        [NotNull]
        private Func<object, string> ValidateRegex([NotNull] Func<string, string> errorGenerator, [NotNull] Regex regex1, [NotNull] string propertyName) {
            ValueValidator.ArgumentNull(regex1, nameof(regex1));
            ValueValidator.ArgumentNull(propertyName, nameof(propertyName));
            ValueValidator.ArgumentNull(errorGenerator, nameof(errorGenerator));
            return o =>
            {
                if (!(o is string s)) {
                    throw ExceptionGenerator.InvalidArgumentType<string>(o?.GetType(), "o");
                }

                return regex1.IsMatch(s) ? string.Empty : errorGenerator(s);
            };
        }

        /// <inheritdoc />
        /// <summary>
        ///     Create a regex validation rule
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="regex"></param>
        public RegexValidationAttribute([NotNull] string propertyName, [NotNull] string regex) {
            ValueValidator.ArgumentNull(regex, nameof(regex));
            ValueValidator.ArgumentNull(propertyName, nameof(propertyName));
            var regex1 = new Regex(regex, RegexOptions.Compiled | RegexOptions.CultureInvariant);
            ErrorGenerator = ValidateRegex(s => $"\"{s}\" is not a valid value for {propertyName}", regex1, propertyName);
            PropertyName = propertyName;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Error string generator
        /// </summary>
        public Func<object, string> ErrorGenerator { get; }

        /// <inheritdoc />
        public string PropertyName { get; }
    }
}