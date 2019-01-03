#region USINGS

using System;
using System.Diagnostics.Tracing;

#endregion

namespace Z.MVVMHelper
{
    public class ExecutingEventArgs : EventArgs
    {
        public ExecutingEventArgs(AsyncTaskStatus status) {
            Status = status;
        }

        public AsyncTaskStatus Status { get; }
    }
}