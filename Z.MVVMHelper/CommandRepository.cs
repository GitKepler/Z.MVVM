#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Interfaces;

#endregion

namespace Z.MVVMHelper
{
    /// <inheritdoc />
    /// <summary>
    ///     Group commands in VMs
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class CommandRepository : List<ICommand>
    {
        /// <summary>
        /// Assign a specific <see cref="IExceptionHandler"/> to every command in the current <see cref="CommandRepository"/>
        /// </summary>
        /// <param name="handler">Exception handler</param>
        public void AssignExceptionHandler([CanBeNull] IExceptionHandler handler) {
            foreach (ICommand command in this.Where(c => !(c is null))) {
                command.ExceptionHandler = handler;
            }
        }
    }
}