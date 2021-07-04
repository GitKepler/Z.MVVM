#region USINGS

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Z.MVVMHelper.Interfaces;

#endregion

namespace Z.MVVMHelper
{
    public abstract partial class VmBase
    {
        
        private ConcurrentDictionary<string, List<IValidator>> ValidationAttributes { get; } = new ConcurrentDictionary<string, List<IValidator>>();

        
        private ConcurrentDictionary<string, IReadOnlyList<string>> ValidationErrors { get; } = new ConcurrentDictionary<string, IReadOnlyList<string>>();

        /// <summary>
        ///     Validation Errors
        /// </summary>
        
        public IReadOnlyDictionary<string, IReadOnlyList<string>> Errors => (IReadOnlyDictionary<string, IReadOnlyList<string>>) ValidationErrors;


        private void InitializeDataValidator() {
            var props = GetType().GetProperties().SelectMany(p => p.GetCustomAttributes(typeof(IValidator), true).OfType<IValidator>());
            foreach (var validatorAttribute in props) {
                ValidationAttributes.AddOrUpdate(
                    validatorAttribute.PropertyName,
                    new List<IValidator> {validatorAttribute},
                    (s, list) =>
                    {
                        list.Add(validatorAttribute);
                        return list;
                    });
            }
        }

        /// <summary>
        ///     Register a custom <see cref="IValidator" />
        /// </summary>
        /// <param name="validator">The validator</param>
        protected void RegisterValidator(IValidator validator) {
            if (validator is null) throw new ArgumentNullException(nameof(validator));

            ValidationAttributes.AddOrUpdate(
                validator.PropertyName,
                new List<IValidator> {validator},
                (s, list) =>
                {
                    list.Add(validator);
                    return list;
                });
        }

        
        internal IReadOnlyList<string> FetchErrors(string? property) {

            var errors = new List<string>();
            if (property is null) 
                return errors;
            var exist = ValidationAttributes.TryGetValue(property, out var attr);
            if (!exist || attr is null) {
                return errors;
            }
            
            var value = GetType().GetProperty(property)?.GetValue(this);
            foreach (var validator in attr) {
                var res = validator.ErrorGenerator(value);
                if (string.IsNullOrWhiteSpace(res)) {
                    continue;
                }

                errors.Add(res);
            }

            return errors;
        }
    }
}