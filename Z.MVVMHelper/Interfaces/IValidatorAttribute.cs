#region USINGS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

#endregion

namespace Z.MVVMHelper.Interfaces
{
    /// <summary>
    ///     Interface for validator attributes
    /// </summary>
    public interface IValidatorAttribute
    {
        /// <summary>
        ///     Validator
        /// </summary>
        [NotNull]
        Func<object, string> ErrorGenerator { get; }

        /// <summary>
        /// Name of the property the attribute is applied to
        /// </summary>
        [NotNull]
        string PropertyName { get; }
    }
}