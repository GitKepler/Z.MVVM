#region USINGS

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
    public abstract class CommandBase : VmBase, ICommand
    {
        /// <summary>
        ///     Predicate to ensure a <see cref="CommandBase" /> or any derived class is always enabled
        /// </summary>
        [NotNull] public static readonly Predicate<object> AlwaysExecute = _ => true;

        [NotNull] private readonly Predicate<object> _canExecute;

        [NotNull] private readonly ConcurrentDictionary<VmBase, List<string>> _propertiesBindings = new ConcurrentDictionary<VmBase, List<string>>();

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
        ///     Add a binding to a property
        /// </summary>
        /// <typeparam name="TVm"></typeparam>
        /// <param name="viewmodel"></param>
        /// <param name="propertyPath"></param>
        public void BindToProperty<TVm>([NotNull] TVm viewmodel, [NotNull] string propertyPath) where TVm : VmBase {
            ValueValidator.ArgumentNull(viewmodel, nameof(viewmodel));
            ValueValidator.ArgumentNull(propertyPath, nameof(propertyPath));
            if (!_propertiesBindings.ContainsKey(viewmodel)) {
                viewmodel.PropertyChanged += PropertyBindingValueModified;
            }

            _propertiesBindings.AddOrUpdate(
                viewmodel,
                new List<string> {propertyPath},
                (vm, list) =>
                {
                    if (!(list?.Contains(propertyPath) ?? true)) {
                        list.Add(propertyPath);
                    }

                    return list;
                });
        }

        private void PropertyBindingValueModified([NotNull] object sender, [NotNull] PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(Error)) {
                Refresh();
            }
        }

        /// <summary>
        ///     Remove a binding to a property
        /// </summary>
        /// <typeparam name="TVm"></typeparam>
        /// <param name="viewmodel"></param>
        /// <param name="propertyPath"></param>
        public void RemoveBindingToProperty<TVm>([NotNull] TVm viewmodel, [NotNull] string propertyPath) where TVm : VmBase {
            ValueValidator.ArgumentNull(viewmodel, nameof(viewmodel));
            ValueValidator.ArgumentNull(propertyPath, nameof(propertyPath));
            _propertiesBindings.AddOrUpdate(
                viewmodel,
                new List<string>(),
                (vm, list) =>
                {
                    if (list?.Contains(propertyPath) ?? false) {
                        list.RemoveAll(p => p == propertyPath);
                    }

                    return list;
                });
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
            var normal = _canExecute(parameter) && (!IsRunning || AllowMultipleExecutions);
            foreach (KeyValuePair<VmBase, List<string>> propertiesBinding in _propertiesBindings) {
                if (propertiesBinding.Value is null) {
                    continue;
                }

                foreach (string s in propertiesBinding.Value) {
                    normal &= string.IsNullOrWhiteSpace(propertiesBinding.Key?.Errors[s]);
                    if (!normal) {
                        break;
                    }
                }

                if (!normal) {
                    break;
                }
            }

            return normal;
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