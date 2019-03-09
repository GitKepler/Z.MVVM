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
            ErrorGenerator = s => !(s?.ToString() is null) && regex1.IsMatch(s?.ToString() ?? string.Empty) ? string.Empty : errorGenerator.Invoke(s?.ToString()) ?? $"\"{s}\" is not a valid value for {propertyName}";
            PropertyName = propertyName;
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
            ErrorGenerator = s => !(s?.ToString() is null) && regex1.IsMatch(s?.ToString() ?? string.Empty) ? string.Empty : $"\"{s}\" is not a valid value for {propertyName}";
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