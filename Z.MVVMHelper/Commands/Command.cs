#region USINGS

using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Z.MVVMHelper.Interfaces;
using Z.MVVMHelper.Internals;
using ICommand = Z.MVVMHelper.Interfaces.ICommand;

#endregion

namespace Z.MVVMHelper
{
    /// <inheritdoc />
    /// <summary>
    ///     Synchronous MVVM Command
    /// </summary>
    /// <typeparam name="TParam">Type of the expected</typeparam>
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    // ReSharper disable once InconsistentNaming
    public class Command<TParam> : ICommand
    {
        /// <summary>
        ///     Always enable the command
        /// </summary>
        [NotNull] public static readonly Predicate<TParam> AlwaysEnabled = _ => true;

        [NotNull] private readonly Predicate<TParam> _canExecute;
        [NotNull] private readonly Action<TParam> _execute;
        private bool? _isEnabled;

        /// <summary>
        ///     Create a <see cref="Command" /> with custom values for every field taking in argument the binding
        ///     parameters
        /// </summary>
        /// <param name="canExecute"><see cref="Predicate{T}" /> determining whether the command can be executed</param>
        /// <param name="execute"><see cref="Action{T}" /> determining the what the command is doing</param>
        public Command([NotNull] Predicate<TParam> canExecute, [NotNull] Action<TParam> execute) {
            _execute = execute ?? throw ExceptionGenerator.ArgumentNull(nameof(execute));
            _canExecute = canExecute ?? throw ExceptionGenerator.ArgumentNull(nameof(canExecute));
        }

        /// <inheritdoc />
        /// <summary>
        ///     Create a <see cref="Command" /> with custom values for the action to execute and taking in argument the
        ///     binding parameters
        /// </summary>
        /// <param name="execute"><see cref="Action{T}" /> determining the what the command is doing</param>
        public Command([NotNull] Action<TParam> execute) : this(AlwaysEnabled, execute) {
            if (execute is null) {
                throw ExceptionGenerator.ArgumentNull(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Create a <see cref="Command" /> with custom values for every field
        /// </summary>
        /// <param name="canExecute"><see cref="Func{TResult}" /> determining whether the command can be executed</param>
        /// <param name="execute"><see cref="Action" /> determining the what the command is doing</param>
        public Command([NotNull] Func<bool> canExecute, [NotNull] Action execute) : this(
            _ => canExecute(),
            _ => canExecute()) {
            if (canExecute is null) {
                throw ExceptionGenerator.ArgumentNull(nameof(canExecute));
            }

            if (execute is null) {
                throw ExceptionGenerator.ArgumentNull(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Create a <see cref="Command" /> with custom values for the action to execute
        /// </summary>
        /// <param name="execute"><see cref="Action" /> determining the what the command is doing</param>
        public Command([NotNull] Action execute) : this(AlwaysEnabled, _ => execute()) {
            if (execute is null) {
                throw ExceptionGenerator.ArgumentNull(nameof(execute));
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
        ///     Event triggered whenever <see cref="M:Z.MVVMHelper.Command`1.CanExecute(System.Object)" /> might return another
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
                switch (parameter) {
                    case TParam t:
                        return _canExecute(t);

                    case null:
                        return _canExecute(default(TParam));
                }
            } else {
                bool val = _isEnabled.Value;
                _isEnabled = null;
                return val;
            }

            throw ExceptionGenerator.InvalidArgumentType<TParam>(parameter?.GetType(), nameof(parameter));
        }

        /// <inheritdoc />
        /// <summary>
        ///     Execute the command with the binding parameter as argument
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute([CanBeNull] object parameter) {
            if (!(parameter is TParam) && !(parameter is null)) {
                throw ExceptionGenerator.InvalidArgumentType<TParam>(parameter?.GetType(), nameof(parameter));
            }

            try {
                if (parameter is null) {
                    _execute(default(TParam));
                } else {
                    _execute((TParam) parameter);
                }
            } catch (Exception ex) when (!(ExceptionHandler is null)) {
                ExceptionHandler.HandleException(ex);
            }

            throw ExceptionGenerator.InvalidArgumentType<TParam>(parameter?.GetType(), nameof(parameter));
        }

        /// <inheritdoc />
        /// <summary>
        ///     Indicate that the command's <see cref="M:Z.MVVMHelper.Command`1.CanExecute(System.Object)" /> return value might
        ///     have changed
        /// </summary>
        public void Refresh() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///     Synchronous MVVM Command
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    // ReSharper disable once InconsistentNaming
    public class Command : Command<object>
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="canExecute"></param>
        /// <param name="execute"></param>
        public Command([NotNull] Predicate<object> canExecute, [NotNull] Action<object> execute) : base(
            canExecute,
            execute) {
            if (canExecute is null) {
                throw ExceptionGenerator.ArgumentNull(nameof(canExecute));
            }

            if (execute is null) {
                throw ExceptionGenerator.ArgumentNull(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="canExecute"></param>
        /// <param name="execute"></param>
        public Command([NotNull] Func<bool> canExecute, [NotNull] Action execute) : base(
            _ => canExecute(),
            _ => canExecute()) {
            if (canExecute is null) {
                throw ExceptionGenerator.ArgumentNull(nameof(canExecute));
            }

            if (execute is null) {
                throw ExceptionGenerator.ArgumentNull(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="execute"></param>
        public Command([NotNull] Action execute) : base(AlwaysEnabled, _ => execute()) {
            if (execute is null) {
                throw ExceptionGenerator.ArgumentNull(nameof(execute));
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        /// <param name="execute"></param>
        public Command([NotNull] Action<object> execute) : base(execute) {
            if (execute is null) {
                throw ExceptionGenerator.ArgumentNull(nameof(execute));
            }
        }
    }
}