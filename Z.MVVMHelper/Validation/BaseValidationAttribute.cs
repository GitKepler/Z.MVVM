#region USINGS

using System;

using Z.MVVMHelper.Interfaces;

#endregion

namespace Z.MVVMHelper.Validation
{
    /// <inheritdoc cref="IValidator" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public abstract class BaseValidationAttribute : Attribute, IValidator
    {
        /// <inheritdoc />
        protected BaseValidationAttribute(string propertyName) {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }

        /// <summary>
        ///     Allow null values
        /// </summary>
        public bool AllowNull { get; set; }

        /// <inheritdoc />
        public abstract Func<object?, string> ErrorGenerator { get; }

        /// <inheritdoc />
        public string PropertyName { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <typeparam name="TExpected"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        
        protected string? PreprocessValue<TExpected>(object? value) {
            if (value is null) {
                return AllowNull ? string.Empty : $"Value of {PropertyName} cannot be null";
            }

            if (!(value is TExpected)) {
                throw new ArgumentException("The provided argument is not of the correct type.", nameof(value));
            }

            return null;
        }
    }
}