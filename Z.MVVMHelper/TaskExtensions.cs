#region USINGS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Interfaces;

#endregion

namespace Z.MVVMHelper
{
    internal static class TaskExtensions
    {
        public static async void FireAndForget([NotNull] this Task task, [CanBeNull] IExceptionHandler handler) {
            try {
                await task;
            } catch (Exception e) {
                handler?.HandleException(e);
            }
        }
    }
}