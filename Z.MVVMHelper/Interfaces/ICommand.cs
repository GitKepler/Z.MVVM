#region USINGS

#endregion

namespace Z.MVVMHelper.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    ///     Simple command (both synchronous &amp; asynchronous)
    /// </summary>
    public interface ICommand : System.Windows.Input.ICommand
    {
        /// <summary>
        /// If the command is running
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// If a command can be run multiple times simultaneously
        /// </summary>
        bool AllowMultipleExecutions { get; }

        /// <summary>
        ///     The exception handler of the command
        /// </summary>
        
        IExceptionHandler? ExceptionHandler { get; set; }

        /// <summary>
        ///     Trigger a forced refresh of the executable state of the command
        /// </summary>
        void Refresh();
    }
}