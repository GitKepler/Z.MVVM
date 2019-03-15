#region USINGS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Commands;

#endregion

namespace Z.MVVMHelper.Interfaces
{
    /// <inheritdoc />
    public interface ICommandRepository : IDictionary<string, CommandBase>
    {
        /// <summary>
        ///     Assign a specific <see cref="IExceptionHandler" /> to every command in the current <see cref="CommandRepository" />
        /// </summary>
        /// <param name="handler">Exception handler</param>
        void AssignExceptionHandler([CanBeNull] IExceptionHandler handler);
    }
}