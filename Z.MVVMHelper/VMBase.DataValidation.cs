#region USINGS

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Z.MVVMHelper
{
    public abstract partial class VmBase : IDataErrorInfo
    {
        private ConcurrentDictionary<string, string> ValidationErrors { get; } = new ConcurrentDictionary<string, string>();

        /// <summary>
        ///     Validation errors
        /// </summary>
        public string Error => throw new NotImplementedException();

        /// <summary>
        ///     Validation errors by members
        /// </summary>
        /// <param name="columnName">Name of the member</param>
        /// <returns></returns>
        public string this[string columnName] => throw new NotImplementedException();
    }
}