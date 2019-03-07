#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.AsyncTypes;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper.Commands
{
    /// <inheritdoc />
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AsyncCancellableCommand : AsyncCommandBase
    {
        [NotNull] private readonly CancellableAction<object> _action;

        /// <inheritdoc />
        public AsyncCancellableCommand([NotNull] CancellableAction<object> action, [NotNull] Predicate<object> canExecute) : base(canExecute) {
            ValueValidator.ArgumentNull(action, nameof(action));
            AllowMultipleExecutions = false;
            _action = action;
            CommandStarted += (sender, args) => _cancelSource = new CancellationTokenSource();
            CommandEnded += (sender, args) => _cancelSource = null;
        }

        /// <inheritdoc />
        public AsyncCancellableCommand([NotNull] CancellableAction<object> action) : this(action, AlwaysExecute) { }

        /// <inheritdoc />
        public AsyncCancellableCommand([NotNull] CancellableAction action, [NotNull] Predicate<object> canExecute) : this((ct, _) => action(ct), canExecute) { }

        /// <inheritdoc />
        public AsyncCancellableCommand([NotNull] CancellableAction action) : this(action, AlwaysExecute) { }

        #region Overrides of AsyncCommandBase

        [CanBeNull] private CancellationTokenSource _cancelSource;

        /// <summary>
        /// Cancel a command if it is running
        /// </summary>
        public void Cancel() {
            _cancelSource?.Cancel();
        }

        /// <inheritdoc />
        protected override async Task RunAsynchronously(object parameter) {
            await _action(_cancelSource.Token, parameter);
        }

        #endregion
    }
}