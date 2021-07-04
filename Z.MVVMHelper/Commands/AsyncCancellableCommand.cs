#region USINGS

using System;
using System.Threading;
using System.Threading.Tasks;

using Z.MVVMHelper.AsyncTypes;

#endregion

namespace Z.MVVMHelper.Commands
{
    /// <inheritdoc />
    public class AsyncCancellableCommand : AsyncCommandBase
    {
        private readonly CancellableAction<object?> _action;

        /// <inheritdoc />
        public AsyncCancellableCommand(CancellableAction<object?> action, Predicate<object?> canExecute) : base(canExecute) {
            if (action is null) throw new ArgumentNullException(nameof(action));

            AllowMultipleExecutions = false;
            _action = action;
            CommandStarted += (sender, args) => _cancelSource = new CancellationTokenSource();
            _cancelSource = new CancellationTokenSource();
        }

        /// <inheritdoc />
        public AsyncCancellableCommand(CancellableAction<object?> action) : this(action, AlwaysExecute) { }

        /// <inheritdoc />
        public AsyncCancellableCommand(CancellableAction action, Predicate<object?> canExecute) : this((ct, _) => action(ct), canExecute) { }

        /// <inheritdoc />
        public AsyncCancellableCommand(CancellableAction action) : this(action, AlwaysExecute) { }

        #region Overrides of AsyncCommandBase

        private CancellationTokenSource _cancelSource;

        /// <summary>
        /// Cancel a command if it is running
        /// </summary>
        public void Cancel() {
            _cancelSource?.Cancel();
        }

        /// <summary>
        /// Generate a <see cref="Command"/> to cancel this <see cref="AsyncCancellableCommand"/>
        /// </summary>
        /// <returns>The command</returns>
        
        public Command GenerateCancelCommand() {
            var command = new Command(Cancel, false, _ => IsRunning);
            PropertyChanged += (sender, args) =>
            {
                if (args?.PropertyName == nameof(IsRunning)) {
                    command.Refresh();
                }
            };
            return command;
        }

        /// <inheritdoc />
        protected override async Task RunAsynchronously(object? parameter) {
            await _action(_cancelSource.Token, parameter);
        }

        #endregion
    }
}