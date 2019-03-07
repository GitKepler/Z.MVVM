#region USINGS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

#endregion

namespace Z.MVVMHelper.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract class AsyncCommandBase : CommandBase
    {
        /// <inheritdoc />
        protected AsyncCommandBase([NotNull] Predicate<object> canExecute) : base(canExecute) { }

        #region Overrides of CommandBase

        /// <inheritdoc />
        protected override async void Run([CanBeNull] object parameter) {
            Task res = RunAsynchronously(parameter);
            if (res is null) {
                return;
            }

            await res;
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected abstract Task RunAsynchronously([CanBeNull] object parameter);
    }
}