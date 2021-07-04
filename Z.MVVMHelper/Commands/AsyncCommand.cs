#region USINGS

using System;
using System.Threading.Tasks;

using Z.MVVMHelper.AsyncTypes;

#endregion

namespace Z.MVVMHelper.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class AsyncCommand : AsyncCommandBase
    {
        private readonly AsyncAction<object?> _action;

        /// <inheritdoc />
        public AsyncCommand(AsyncAction<object?> action, bool allowMultipleExecutions, Predicate<object?> canExecute) : base(canExecute) {
            if (action is null) throw new ArgumentNullException(nameof(action));

            AllowMultipleExecutions = allowMultipleExecutions;
            _action = action;
        }


        /// <inheritdoc />
        public AsyncCommand(AsyncAction<object?> action, bool allowMultipleExecutions) : this(action, allowMultipleExecutions, AlwaysExecute) { }

        /// <inheritdoc />
        public AsyncCommand(AsyncAction action, bool allowMultipleExecutions, Predicate<object?> canExecute) : this(_ => action(), allowMultipleExecutions, canExecute) { }

        /// <inheritdoc />
        public AsyncCommand(AsyncAction action, bool allowMultipleExecutions) : this(action, allowMultipleExecutions, AlwaysExecute) { }

        #region Overrides of AsyncCommandBase

        /// <inheritdoc />
        protected override async Task RunAsynchronously(object? parameter) {
            var res = _action(parameter);
            if (res is null) {
                return;
            }

            await res;
        }

        #endregion
    }
}