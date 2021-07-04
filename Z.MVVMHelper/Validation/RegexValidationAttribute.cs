#region USINGS

using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

#endregion

namespace Z.MVVMHelper.Validation
{
    /// <inheritdoc cref="Attribute" />
    /// <summary>
    ///     Regex validation rule
    /// </summary>
    public sealed class RegexValidationAttribute : BaseValidationAttribute
    {
        private readonly Regex _regex;

        /// <inheritdoc />
        /// <summary>
        ///     Create a regex validation rule with a custom error generator
        /// </summary>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="regex">Regex to match</param>
        /// <param name="errorGenerator">Error generator</param>
        public RegexValidationAttribute(string propertyName, string regex, Func<string?, string> errorGenerator) : base(propertyName) {
            if (regex is null) throw new ArgumentNullException(nameof(regex));
            if (errorGenerator is null) throw new ArgumentNullException(nameof(errorGenerator));

            _regex = new Regex(regex, RegexOptions.Compiled | RegexOptions.CultureInvariant);
            ErrorGenerator = ValidateRegex(errorGenerator);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Create a regex validation rule
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="regex"></param>
        public RegexValidationAttribute(string propertyName, string regex) : base(propertyName) {
            if (regex is null) throw new ArgumentNullException(nameof(regex));

            _regex = new Regex(regex, RegexOptions.Compiled | RegexOptions.CultureInvariant);
            ErrorGenerator = ValidateRegex(s => $"\"{s}\" is not a valid value for {propertyName}");
        }

        #region Overrides of BaseValidationAttribute

        /// <inheritdoc />
        public override Func<object?, string> ErrorGenerator { get; }

        #endregion

        
        private Func<object?, string> ValidateRegex(Func<string?, string> errorGenerator) {
            if (errorGenerator is null) throw new ArgumentNullException(nameof(errorGenerator));

            return o =>
            {
                var prep = PreprocessValue<string>(o);
                if (prep is null) {
                    var s = o as string ?? string.Empty;
                    return _regex.IsMatch(s) ? string.Empty : errorGenerator(s);
                }

                return prep;
            };
        }
    }
}