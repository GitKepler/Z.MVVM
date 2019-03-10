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
    }
}