#region USINGS

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    public abstract partial class VmBase : IDataErrorInfo
    {
        [CanBeNull] private string _error;

        [NotNull]
        private ConcurrentDictionary<string, string> ValidationErrors { get; } = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Validation Errors
        /// </summary>
        [NotNull]
        public IReadOnlyDictionary<string, string> Errors => ValidationErrors;


        [NotNull]
        private ConcurrentDictionary<string, List<IValidator>> ValidationAttributes { get; } = new ConcurrentDictionary<string, List<IValidator>>();

        /// <summary>
        ///     If the VM is valid
        /// </summary>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public bool IsValid => string.IsNullOrWhiteSpace(Error);

        /// <inheritdoc />
        /// <summary>
        ///     Validation errors
        /// </summary>
        [CanBeNull]
        public string Error {
            get => _error;
            private set {
                EditProperty(ref _error, value);
                DelegatePropertyChanged(nameof(IsValid));
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Validation errors by members
        /// </summary>
        /// <param name="columnName">Name of the member</param>
        /// <returns></returns>
        [NotNull]
        public string this[[CanBeNull] string columnName] {
            get {
                if (columnName is null) {
                    foreach (KeyValuePair<string, List<IValidator>> validationAttribute in ValidationAttributes) {
                        string _ = this[validationAttribute.Key];
                    }
                }

                object value = GetType().GetProperty(columnName)?.GetValue(this);
                bool isOk = ValidationAttributes.TryGetValue(columnName, out List<IValidator> validators);
                if (!isOk) {
                    return string.Empty;
                }

                string errors = validators
                    .Select(a => a?.ErrorGenerator)
                    .Where(g => !(g is null))
                    .Aggregate(new StringBuilder(), (builder, generator) => builder.AppendLine(generator(value)))
                    .ToString()
                    .Trim();
                ValidationErrors.AddOrUpdate(columnName, errors, (s, s1) => errors);
                Error = string.Join(Environment.NewLine, ValidationErrors.ToArray().Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value)).Select(kvp => $"[{kvp.Key}]{Environment.NewLine}{kvp.Value}"));
                return errors;
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
    }
}