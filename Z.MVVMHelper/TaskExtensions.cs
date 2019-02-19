#region USINGS

using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Interfaces;

#endregion

namespace Z.MVVMHelper
{
    internal static class TaskExtensions
    {
        public static async void FireAndForget([NotNull] this Task task, [CanBeNull] IExceptionHandler handler) {
            if (task == null) {
                throw Internals.ExceptionGenerator.ArgumentNull(nameof(task));
            }

            try {
                await task;
            } catch (Exception e) when (!(handler is null)) {
                handler.HandleException(e);
            }
        }
    }
}