#region USINGS

using System;
using System.Threading.Tasks;


#endregion

namespace Z.MVVMHelper.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract class AsyncCommandBase : CommandBase
    {
        /// <inheritdoc />
        protected AsyncCommandBase(Predicate<object?> canExecute) : base(canExecute) { }

        #region Overrides of CommandBase

        /// <inheritdoc />
        protected override async void Run(object? parameter, bool catchError) {
            MethodStart();
            try {
                var res = RunAsynchronously(parameter);

                if (res is null) {
                    return;
                }

                await res;
            } catch (TaskCanceledException) {
                // Canceled
            } catch (Exception e) when (catchError) {
                // Need to duplicate code from since async void does not play nicely with exceptions
                ExceptionHandler?.HandleException(e);
            } finally {
                MethodEnd();
            }
        }

        #endregion

        /// <summary>
        /// Content of the method to run
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected abstract Task? RunAsynchronously(object? parameter);
    }
}