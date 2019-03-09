#region USINGS

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Interfaces;

#endregion

namespace Z.MVVMHelper
{
    public abstract partial class VmBase : IDataErrorInfo
    {
        [NotNull]
        private ConcurrentDictionary<string, string> ValidationErrors { get; } = new ConcurrentDictionary<string, string>();

        [NotNull]
        private ConcurrentDictionary<string, List<IValidatorAttribute>> ValidationAttributes { get; } = new ConcurrentDictionary<string, List<IValidatorAttribute>>();

        /// <inheritdoc />
        /// <summary>
        ///     Validation errors
        /// </summary>
        public string Error => throw new NotImplementedException();

        /// <inheritdoc />
        /// <summary>
        ///     Validation errors by members
        /// </summary>
        /// <param name="columnName">Name of the member</param>
        /// <returns></returns>
        public string this[string columnName] => throw new NotImplementedException();

        private void InitializeDataValidator() {
            IEnumerable<IValidatorAttribute> props = GetType().GetProperties().SelectMany(p => p.GetCustomAttributes(typeof(IValidatorAttribute), true).OfType<IValidatorAttribute>());
            foreach (IValidatorAttribute validatorAttribute in props) {
                ValidationAttributes.GetOrAdd(validatorAttribute.PropertyName, new List<IValidatorAttribute>())?.Add(validatorAttribute);
            }
        }
    }
}