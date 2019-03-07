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
        protected EventHandler<EventArgs> CommandEnded;

        protected EventHandler<EventArgs> CommandStarted;

        /// <summary>
        /// </summary>
        /// <param name="canExecute"><see cref="Predicate{T}" /> checking whether the <see cref="CommandBase" /> can be run</param>
        protected CommandBase([NotNull] Predicate<object> canExecute) {
            if (canExecute is null) {
                throw new ArgumentNullException(nameof(canExecute));
            }

            _canExecute = canExecute;
        }

        /// <inheritdoc />
        /// <summary>
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
        /// </summary>
        public void Refresh() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        protected abstract void Run([CanBeNull] object parameter);

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="catchError"></param>
        public void Execute([CanBeNull] object parameter, bool catchError) {
            MethodStart();
            try {
                Run(parameter);
            } catch (Exception ex) when (catchError) {
                ExceptionHandler?.HandleException(ex);
            } finally {
                MethodEnd();
            }
        }

        private void MethodStart() {
            lock (Locker) {
                IsRunning = true;
                Refresh();
            }
        }

        private void MethodEnd() {
            lock (Locker) {
                IsRunning = false;
                Refresh();
            }
        }

        /// <summary>
        /// </summary>
        protected void TriggerCommandStarted() {
            CommandStarted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// </summary>
        protected void TriggerCommandEnded() {
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