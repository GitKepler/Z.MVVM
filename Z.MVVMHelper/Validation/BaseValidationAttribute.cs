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
    /// <inheritdoc cref="IValidator" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public abstract class BaseValidationAttribute : Attribute, IValidator
    {
        /// <inheritdoc />
        protected BaseValidationAttribute([NotNull] string propertyName) {
            ValueValidator.ArgumentNull(propertyName, nameof(propertyName));
            PropertyName = propertyName;
        }

        /// <summary>
        ///     Allow null values
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        public bool AllowNull { get; set; }

        /// <inheritdoc />
        public abstract Func<object, string> ErrorGenerator { get; }

        /// <inheritdoc />
        public string PropertyName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TExpected"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [CanBeNull]
        protected string PreprocessValue<TExpected>([CanBeNull] object value) {
            if (value is null) {
                return AllowNull ? string.Empty : $"Value of {PropertyName} cannot be null";
            }

            if (!(value is TExpected)) {
                throw ExceptionGenerator.InvalidArgumentType<TExpected>(value?.GetType(), "o");
            }

            return null;
        }
    }
}