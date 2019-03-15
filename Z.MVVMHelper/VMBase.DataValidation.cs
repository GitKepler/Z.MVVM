#region USINGS

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Interfaces;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper
{
    public abstract partial class VmBase
    {
        [NotNull]
        private ConcurrentDictionary<string, List<IValidator>> ValidationAttributes { get; } = new ConcurrentDictionary<string, List<IValidator>>();

        [NotNull]
        private ConcurrentDictionary<string, IReadOnlyList<string>> ValidationErrors { get; } = new ConcurrentDictionary<string, IReadOnlyList<string>>();

        /// <summary>
        ///     Validation Errors
        /// </summary>
        [NotNull]
        public IReadOnlyDictionary<string, IReadOnlyList<string>> Errors => ValidationErrors;


        private void InitializeDataValidator() {
            IEnumerable<IValidator> props = GetType().GetProperties().SelectMany(p => p.GetCustomAttributes(typeof(IValidator), true).OfType<IValidator>());
            foreach (IValidator validatorAttribute in props) {
                ValidationAttributes.AddOrUpdate(
                    validatorAttribute.PropertyName,
                    new List<IValidator> {validatorAttribute},
                    (s, list) =>
                    {
                        list?.Add(validatorAttribute);
                        return list;
                    });
            }
        }

        /// <summary>
        ///     Register a custom <see cref="IValidator" />
        /// </summary>
        /// <param name="validator">The validator</param>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        protected void RegisterValidator([NotNull] IValidator validator) {
            ValueValidator.ArgumentNull(validator, nameof(validator));
            ValidationAttributes.AddOrUpdate(
                validator.PropertyName,
                new List<IValidator> {validator},
                (s, list) =>
                {
                    list?.Add(validator);
                    return list;
                });
        }

        [NotNull]
        internal IReadOnlyList<string> FetchErrors([NotNull] string property) {
            var errors = new List<string>();
            List<IValidator> attr = ValidationAttributes[property];
            if (attr is null) {
                return errors;
            }

            ValueValidator.ArgumentNull(property, nameof(property));
            object value = GetType().GetProperty(property)?.GetValue(this);
            foreach (IValidator validator in attr) {
                string res = validator.ErrorGenerator(value);
                if (string.IsNullOrWhiteSpace(res)) {
                    continue;
                }

                errors.Add(res);
            }

            return errors;
        }
    }
}