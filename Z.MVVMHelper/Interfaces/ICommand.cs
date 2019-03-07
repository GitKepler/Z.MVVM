#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

#endregion

namespace Z.MVVMHelper.Interfaces
{
    /// <inheritdoc />
    /// <summary>
    ///     Simple command (both synchronous &amp; asynchronous)
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBeInternal")]
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
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
        [CanBeNull]
        IExceptionHandler ExceptionHandler { get; set; }

        /// <summary>
        ///     Trigger a forced refresh of the executable state of the command
        /// </summary>
        void Refresh();
    }
}