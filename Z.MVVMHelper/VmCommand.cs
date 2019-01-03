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
    public class VmCommand<TParam> : ICommand
    {
        [NotNull] protected static readonly Predicate<TParam> AlwaysEnabled = _ => true;
        [NotNull] private readonly Predicate<TParam> _canExecute;
        [NotNull] private readonly Action<TParam> _execute;

        public VmCommand([NotNull] Predicate<TParam> canExecute, [NotNull] Action<TParam> execute) {
            _execute = execute;
            _canExecute = canExecute;
        }

        public VmCommand([NotNull] Action<TParam> execute) : this(AlwaysEnabled, execute) { }

        public VmCommand([NotNull] Func<bool> canExecute, [NotNull] Action execute) : this(
            _ => canExecute(),
            _ => canExecute()) { }

        public VmCommand([NotNull] Action execute) : this(AlwaysEnabled, _ => execute()) { }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute([CanBeNull] object parameter) {
            if (parameter is TParam t) {
                return _canExecute(t);
            }

            throw new ArgumentException(
                $"{nameof(parameter)} ({parameter?.GetType().Name ?? "null"}) is incompatible with {nameof(TParam)}");
        }

        public void Execute([CanBeNull] object parameter) {
            if (parameter is TParam t) {
                _execute(t);
            }

            throw new ArgumentException(
                $"{nameof(parameter)} ({parameter?.GetType().Name ?? "null"}) is incompatible with {nameof(TParam)}");
        }

        public void Refresh() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

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