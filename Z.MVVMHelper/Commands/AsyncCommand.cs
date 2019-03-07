#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.AsyncTypes;

#endregion

namespace Z.MVVMHelper.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AsyncCommand : AsyncCommandBase
    {
        [NotNull] private readonly AsyncAction<object> _action;

        /// <inheritdoc />
        public AsyncCommand([NotNull] AsyncAction<object> action, bool allowMultipleExecutions, [NotNull] Predicate<object> canExecute) : base(canExecute) {
            AllowMultipleExecutions = allowMultipleExecutions;
            _action = action;
        }


        /// <inheritdoc />
        public AsyncCommand([NotNull] AsyncAction<object> action, bool allowMultipleExecutions) : this(action, allowMultipleExecutions, AlwaysExecute) { }

        /// <inheritdoc />
        public AsyncCommand([NotNull] AsyncAction action, bool allowMultipleExecutions, [NotNull] Predicate<object> canExecute) : this(_ => action(), allowMultipleExecutions, canExecute) { }

        /// <inheritdoc />
        public AsyncCommand([NotNull] AsyncAction action, bool allowMultipleExecutions) : this(action, allowMultipleExecutions, AlwaysExecute) { }

        #region Overrides of AsyncCommandBase

        /// <inheritdoc />
        protected override async Task RunAsynchronously(object parameter) {
            Task res = _action(parameter);
            if (res is null) {
                return;
            }

            await res;
        }

        #endregion
    }
}