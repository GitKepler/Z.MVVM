using System;

namespace Z.MVVMHelper.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract class SynchronousCommandBase : CommandBase
    {
        /// <inheritdoc />
        protected SynchronousCommandBase(Predicate<object?> canExecute) : base(canExecute) { }

        #region Overrides of CommandBase

        /// <inheritdoc />
        protected override void Run(object? parameter, bool catchError) {
            MethodStart();
            try {
                RunSynchronously(parameter);
            } finally {
                MethodEnd();
            }
        }

        /// <summary>
        /// Content of the method to run
        /// </summary>
        /// <param name="parameter"></param>
        protected abstract void RunSynchronously(object? parameter);

        #endregion
    }
}
