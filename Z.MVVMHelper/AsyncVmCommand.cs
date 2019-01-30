#region USINGS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using JetBrains.Annotations;

#endregion

namespace Z.MVVMHelper
{
    public class AsyncVmCommand<TParam> : Interfaces.ICommand
    {
        [NotNull] protected static readonly Func<TParam, bool> AlwaysEnabled = _ => true;
        [NotNull] private readonly Func<TParam, bool> _canExecute;
        [NotNull] private readonly Func<TParam, Task> _execute;
        private bool? _isEnabled;

        public AsyncVmCommand([NotNull] Func<TParam, bool> canExecute, [NotNull] Func<TParam, Task> execute) {
            _canExecute = canExecute;
            _execute = execute;
        }

        public AsyncVmCommand([NotNull] Func<bool> canExecute, [NotNull] Func<Task> execute) : this(
            _ => canExecute(),
            _ => execute()) { }

        public AsyncVmCommand([NotNull] Func<Task> execute) : this(AlwaysEnabled, _ => execute()) { }
        public AsyncVmCommand([NotNull] Func<TParam, Task> execute) : this(AlwaysEnabled, execute) { }

        public bool IsEnabled {
            get => CanExecute(null);
            set {
                _isEnabled = value;
                Refresh();
            }
        }

        [CanBeNull]
        public IExceptionHandler ExceptionHandler { get; set; }

        public event EventHandler CanExecuteChanged;

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

        public void Refresh() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<ExecutingEventArgs> Executing;
    }

    public class AsyncVmCommand : AsyncVmCommand<object>
    {
        public AsyncVmCommand([NotNull] Func<object, bool> canExecute, [NotNull] Func<object, Task> execute) : base(
            canExecute,
            execute) { }

        public AsyncVmCommand([NotNull] Func<bool> canExecute, [NotNull] Func<Task> execute) : base(
            _ => canExecute(),
            _ => execute()) { }

        public AsyncVmCommand([NotNull] Func<Task> execute) : base(AlwaysEnabled, _ => execute()) { }
        public AsyncVmCommand([NotNull] Func<object, Task> execute) : base(AlwaysEnabled, execute) { }
    }
}