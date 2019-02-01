#region USINGS

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace Z.MVVMHelper
{
    /// <inheritdoc />
    /// <summary>
    ///     Indicate the status of a <see cref="T:System.Threading.Tasks.Task" /> completion
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public sealed class ExecutingEventArgs : EventArgs
    {
        /// <inheritdoc />
        /// <summary>
        ///     Create a new instance of <see cref="T:Z.MVVMHelper.ExecutingEventArgs" />
        /// </summary>
        /// <param name="status">The current status of the <see cref="T:System.Threading.Tasks.Task" /></param>
        public ExecutingEventArgs(AsyncTaskStatus status) {
            Status = status;
        }

        /// <summary>
        ///     Current status of the awaited element
        /// </summary>
        public AsyncTaskStatus Status { get; }
    }
}