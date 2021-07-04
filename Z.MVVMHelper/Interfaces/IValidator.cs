#region USINGS

using System;

#endregion

namespace Z.MVVMHelper.Interfaces
{
    /// <summary>
    ///     Interface for validator attributes
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        ///     Validator
        /// </summary>
        
        Func<object?, string> ErrorGenerator { get; }

        /// <summary>
        /// Name of the property the attribute is applied to
        /// </summary>
        
        string PropertyName { get; }
    }
}