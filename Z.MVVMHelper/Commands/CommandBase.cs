#region USINGS

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Interfaces;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper.Commands
{
    /// <summary>
    ///     Base class for every command
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public abstract class CommandBase : VMBase, ICommand
    {
        /// <summary>
        ///     Predicate to ensure a <see cref="CommandBase" /> or any derived class is always enabled
        /// </summary>
        [NotNull] public static readonly Predicate<object> AlwaysExecute = _ => true;

        [NotNull] private readonly Predicate<object> _canExecute;

        private bool _isRunning;

        /// <summary>
        ///     Event triggered when the execution of a command finishes
        /// </summary>
        protected EventHandler<EventArgs> CommandEnded;

        /// <summary>
        ///     Event triggered when the execution of a command start
        /// </summary>
        protected EventHandler<EventArgs> CommandStarted;

        /// <summary>
        /// </summary>
        /// <param name="canExecute"><see cref="Predicate{T}" /> checking whether the <see cref="CommandBase" /> can be run</param>
        protected CommandBase([NotNull] Predicate<object> canExecute) {
            ValueValidator.ArgumentNull(canExecute, nameof(canExecute));

            _canExecute = canExecute;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Check whether a command can be run multiple times in parallel
        /// </summary>
        public bool AllowMultipleExecutions { get; protected set; }

        /// <inheritdoc />
        /// <summary>
        ///     If the <see cref="T:Z.MVVMHelper.Commands.CommandBase" /> is running
        /// </summary>
        public bool IsRunning { get => _isRunning; protected set => EditProperty(ref _isRunning, value); }

        /// <inheritdoc />
        /// <summary>
        ///     Exception handler for the <see cref="T:Z.MVVMHelper.Commands.CommandBase" />
        /// </summary>
        public IExceptionHandler ExceptionHandler { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Refresh the <see cref="CommandBase" /> Enabled status
        /// </summary>
        public void Refresh() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Content of the command to run
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="catchError">If the errors needs to be thrown or not</param>
        protected abstract void Run([CanBeNull] object parameter, bool catchError);

        /// <summary>
        ///     Execute the method from user code
        /// </summary>
        /// <param name="parameter">The arguments if the method takes any</param>
        /// <param name="catchError">If the errors needs to be thrown or not</param>
        public void Execute([CanBeNull] object parameter, bool catchError) {
            try {
                Run(parameter, catchError);
            } catch (Exception ex) when (catchError) {
                ExceptionHandler?.HandleException(ex);
            }
        }

        /// <summary>
        ///     To execute before the beginning of the method
        /// </summary>
        protected void MethodStart() {
            lock (Locker) {
                TriggerCommandStarted();
                IsRunning = true;
                Refresh();
            }
        }

        /// <summary>
        ///     To execute after the end of the method (preferably in a finally block)
        /// </summary>
        protected void MethodEnd() {
            lock (Locker) {
                TriggerCommandEnded();
                IsRunning = false;
                Refresh();
            }
        }

        private void TriggerCommandStarted() {
            CommandStarted?.Invoke(this, EventArgs.Empty);
        }

        private void TriggerCommandEnded() {
            CommandEnded?.Invoke(this, EventArgs.Empty);
        }

        #region Implementation of ICommand

        /// <inheritdoc />
        public bool CanExecute([CanBeNull] object parameter) {
            return _canExecute(parameter) && (!IsRunning || AllowMultipleExecutions);
        }

        /// <summary>
        /// </summary>
        [NotNull] protected readonly object Locker = new object();

        /// <inheritdoc />
        public void Execute([CanBeNull] object parameter) {
            Execute(parameter, true);
        }

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;

        #endregion
    }
}