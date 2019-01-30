#region USINGS

using System;

#endregion

namespace Z.MVVMHelper.Interfaces
{
    /// <summary>
    ///     Handler for different exceptions
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        ///     Handle an exception
        /// </summary>
        /// <typeparam name="TExc">The type of the exception to handle</typeparam>
        /// <param name="exception">The exception to handle</param>
        void HandleException<TExc>(TExc exception) where TExc : Exception;
    }
}