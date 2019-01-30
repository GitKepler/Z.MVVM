#region USINGS

using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using ICommand = Z.MVVMHelper.Interfaces.ICommand;

#endregion

namespace Z.MVVMHelper
{
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    public class VmCommand<TParam> : ICommand
    {
        [NotNull] protected static readonly Predicate<TParam> AlwaysEnabled = _ => true;
        [NotNull] private readonly Predicate<TParam> _canExecute;
        [NotNull] private readonly Action<TParam> _execute;
        private bool? _isEnabled;

        public VmCommand([NotNull] Predicate<TParam> canExecute, [NotNull] Action<TParam> execute) {
            _execute = execute;
            _canExecute = canExecute;
        }

        public VmCommand([NotNull] Action<TParam> execute) : this(AlwaysEnabled, execute) { }

        public VmCommand([NotNull] Func<bool> canExecute, [NotNull] Action execute) : this(
            _ => canExecute(),
            _ => canExecute()) { }

        public VmCommand([NotNull] Action execute) : this(AlwaysEnabled, _ => execute()) { }

        public bool IsEnabled {
            get => CanExecute(null);
            set {
                _isEnabled = value;
                Refresh();
            }
        }

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

            try {
                _execute(t);
            } catch (Exception ex) {
                ExceptionHandler?.HandleException(ex);
            }

            throw new ArgumentException(
                $"{nameof(parameter)} ({parameter.GetType().Name}) is incompatible with {nameof(TParam)}");
        }

        public void Refresh() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class VmCommand : VmCommand<object>
    {
        public VmCommand([NotNull] Predicate<object> canExecute, [NotNull] Action<object> execute) : base(
            canExecute,
            execute) { }

        public VmCommand([NotNull] Func<bool> canExecute, [NotNull] Action execute) : base(
            _ => canExecute(),
            _ => canExecute()) { }

        public VmCommand([NotNull] Action execute) : base(AlwaysEnabled, _ => execute()) { }
        public VmCommand([NotNull] Action<object> execute) : base(execute) { }
    }
}