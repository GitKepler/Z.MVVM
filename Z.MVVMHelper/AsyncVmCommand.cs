#region USINGS

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.AsyncTypes;
using Z.MVVMHelper.Interfaces;
using ICommand = Z.MVVMHelper.Interfaces.ICommand;

#endregion

namespace Z.MVVMHelper
{
    /// <inheritdoc />
    /// <summary>
    ///     Asynchronous MVVM Command
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    [SuppressMessage("ReSharper", "EventNeverSubscribedTo.Global")]
    // ReSharper disable once InconsistentNaming
    public class AsyncVMCommand<TParam> : ICommand
    {
        /// <summary>
        ///     Always enable the command
        /// </summary>
        [NotNull] public static readonly Predicate<TParam> AlwaysEnabled = _ => true;

        [NotNull] private readonly Predicate<TParam> _canExecute;
        [NotNull] private readonly AsyncAction<TParam> _execute;
        private bool? _isEnabled;

        /// <summary>
        ///     Create a <see cref="AsyncVMCommand{TParam}" /> with custom values for every field taking in argument the binding
        ///     parameters
        /// </summary>
        /// <param name="canExecute"><see cref="Predicate{T}" /> determining whether the command can be executed</param>
        /// <param name="execute">
        ///     <see cref="Func{TResult}" /> determining the what the command is doing. The returned value is the
        ///     <see cref="Task" /> to await
        /// </param>
        public AsyncVMCommand([NotNull] Predicate<TParam> canExecute, [NotNull] AsyncAction<TParam> execute) {
            _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        /// <inheritdoc />
        /// <summary>
        ///     Create a <see cref="AsyncVMCommand{TParam}" /> with custom values for every field
        /// </summary>
        /// <param name="canExecute"><see cref="Func{TResult}" /> determining whether the command can be executed</param>
        /// <param name="execute">
        ///     <see cref="Func{TResult}" /> determining the what the command is doing. The returned value is the
        ///     <see cref="Task" /> to await
        /// </param>
        public AsyncVMCommand([NotNull] Func<bool> canExecute, [NotNull] AsyncAction execute) : this(
            _ => canExecute(),
            _ => execute()) {
            if (canExecute == null) {
                throw new ArgumentNullException(nameof(canExecute));
            }

            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Create a <see cref="AsyncVMCommand{TParam}" /> with custom values for the action to execute and taking in argument
        ///     the
        ///     binding parameters
        /// </summary>
        /// <param name="execute">
        ///     <see cref="Func{TResult}" /> determining the what the command is doing. The returned value is the
        ///     <see cref="Task" /> to await
        /// </param>
        public AsyncVMCommand([NotNull] AsyncAction execute) : this(AlwaysEnabled, _ => execute()) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Create a <see cref="VMCommand" /> with custom values for the action to execute
        /// </summary>
        /// <param name="execute">
        ///     <see cref="Func{TResult}" /> determining the what the command is doing. The returned value is the
        ///     <see cref="Task" /> to await
        /// </param>
        public AsyncVMCommand([NotNull] AsyncAction<TParam> execute) : this(AlwaysEnabled, execute) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Check whether the command can be run and force an override (one-time)
        /// </summary>
        public bool IsEnabled {
            get => CanExecute(null);
            set {
                _isEnabled = value;
                Refresh();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Handler whenever a method throws an exception
        /// </summary>
        public IExceptionHandler ExceptionHandler { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Event triggered whenever <see cref="M:Z.MVVMHelper.VMCommand`1.CanExecute(System.Object)" /> might return another
        ///     value
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc />
        /// <summary>
        ///     Returns whether the command might or not be able to run depending on the binding parameter
        /// </summary>
        /// <param name="parameter">Binding parameter</param>
        /// <returns>Whether the command can run</returns>
        public bool CanExecute([CanBeNull] object parameter) {
            if (_isEnabled is null) {
                if (parameter is TParam t) {
                    return _canExecute(t);
                }
            } else {
                return _isEnabled.Value;
            }

            throw new ArgumentException(
                $"{nameof(parameter)} ({parameter?.GetType().Name ?? "null"}) is incompatible with {nameof(TParam)}");
        }

        /// <inheritdoc />
        /// <summary>
        ///     Execute the command with the binding parameter as argument
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute([CanBeNull] object parameter) {
            if (!(parameter is TParam t)) {
                throw new ArgumentException(
                    $"{nameof(parameter)} ({parameter?.GetType().Name ?? "null"}) is incompatible with {nameof(TParam)}");
            }

            Executing?.Invoke(this, new ExecutingEventArgs(AsyncTaskStatus.Starting));
            Task awaitable = _execute(t);
            Executing?.Invoke(this, new ExecutingEventArgs(AsyncTaskStatus.Started));
            if (awaitable is null) {
                Executing?.Invoke(this, new ExecutingEventArgs(AsyncTaskStatus.Ended));
            } else {
                awaitable.ContinueWith(a => Executing?.Invoke(this, new ExecutingEventArgs(AsyncTaskStatus.Ended)));
                awaitable.FireAndForget(ExceptionHandler);
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Indicate that the command's <see cref="M:Z.MVVMHelper.AsyncVMCommand`1.CanExecute(System.Object)" /> return value
        ///     might have changed
        /// </summary>
        public void Refresh() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Current state of execution of the executed <see cref="Task" />
        /// </summary>
        public event EventHandler<ExecutingEventArgs> Executing;
    }

    /// <inheritdoc />
    /// <summary>
    ///     Asynchronous MVVM Command
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    // ReSharper disable once InconsistentNaming
    public class AsyncVMCommand : AsyncVMCommand<object>
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="canExecute"></param>
        /// <param name="execute"></param>
        public AsyncVMCommand([NotNull] Predicate<object> canExecute, [NotNull] AsyncAction<object> execute) : base(
            canExecute,
            execute) {
            if (canExecute == null) {
                throw new ArgumentNullException(nameof(canExecute));
            }

            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="canExecute"></param>
        /// <param name="execute"></param>
        public AsyncVMCommand([NotNull] Func<bool> canExecute, [NotNull] AsyncAction execute) : base(
            _ => canExecute(),
            _ => execute()) {
            if (canExecute == null) {
                throw new ArgumentNullException(nameof(canExecute));
            }

            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="execute"></param>
        public AsyncVMCommand([NotNull] AsyncAction execute) : base(AlwaysEnabled, _ => execute()) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="execute"></param>
        public AsyncVMCommand([NotNull] AsyncAction<object> execute) : base(AlwaysEnabled, execute) {
            if (execute == null) {
                throw new ArgumentNullException(nameof(execute));
            }
        }
    }
}