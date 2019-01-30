#region USINGS

using System.Threading.Tasks;

#endregion

namespace Z.MVVMHelper
{
    /// <summary>
    ///     Status of a <see cref="Task" />
    /// </summary>
    public enum AsyncTaskStatus
    {
        /// <summary>
        ///     The task is starting
        /// </summary>
        Starting,

        /// <summary>
        ///     The task has not yet been awaited
        /// </summary>
        Started,

        /// <summary>
        ///     The task has been awaited
        /// </summary>
        Ended
    }
}