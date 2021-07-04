#region USINGS

using System;

#endregion

namespace Z.MVVMHelper.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public class Command : SynchronousCommandBase
    {
        private readonly Action<object?> _action;

        /// <inheritdoc />
        public Command(Action<object?> action, bool allowMultipleExecutions, Predicate<object?> canExecute) : base(canExecute) {
            if (action is null) throw new ArgumentNullException(nameof(action));

            _action = action;
            AllowMultipleExecutions = allowMultipleExecutions;
        }

        /// <inheritdoc />
        public Command(Action<object?> action, bool allowMultipleExecutions) : this(action, allowMultipleExecutions, AlwaysExecute) { }

        /// <inheritdoc />
        public Command(Action action, bool allowMultipleExecutions, Predicate<object?> canExecute) : this(_ => action(), allowMultipleExecutions, canExecute) { }

        /// <inheritdoc />
        public Command(Action action, bool allowMultipleExecutions) : this(action, allowMultipleExecutions, AlwaysExecute) { }

        #region Overrides of CommandBase

        /// <inheritdoc />
        protected override void RunSynchronously(object? parameter) {
            _action(parameter);
        }

        #endregion
    }
}