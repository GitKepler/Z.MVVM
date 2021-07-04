#region USINGS

using System.Collections.Generic;

using Z.MVVMHelper.Commands;

#endregion

namespace Z.MVVMHelper.Interfaces
{
    /// <inheritdoc />
    public interface ICommandRepository : IDictionary<string, CommandBase?>
    {
        /// <summary>
        ///     Assign a specific <see cref="IExceptionHandler" /> to every command in the current <see cref="CommandRepository" />
        /// </summary>
        /// <param name="handler">Exception handler</param>
        void AssignExceptionHandler(IExceptionHandler handler);
    }
}