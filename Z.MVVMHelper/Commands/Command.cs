#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

#endregion

namespace Z.MVVMHelper.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Command : SynchronousCommandBase
    {
        [NotNull] private readonly Action<object> _action;

        /// <inheritdoc />
        public Command([NotNull] Action<object> action, bool allowMultipleExecutions, [NotNull] Predicate<object> canExecute) : base(canExecute) {
            _action = action;
            AllowMultipleExecutions = allowMultipleExecutions;
        }

        /// <inheritdoc />
        public Command([NotNull] Action<object> action, bool allowMultipleExecutions) : this(action, allowMultipleExecutions, AlwaysExecute) { }

        /// <inheritdoc />
        public Command([NotNull] Action action, bool allowMultipleExecutions, [NotNull] Predicate<object> canExecute) : this(_ => action(), allowMultipleExecutions, canExecute) { }

        /// <inheritdoc />
        public Command([NotNull] Action action, bool allowMultipleExecutions) : this(action, allowMultipleExecutions, AlwaysExecute) { }

        #region Overrides of CommandBase

        /// <inheritdoc />
        protected override void RunSynchronously(object parameter) {
            _action(parameter);
        }

        #endregion
    }
}