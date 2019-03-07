#region USINGS

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

#endregion

namespace Z.MVVMHelper
{
    /// <inheritdoc cref="INotifyPropertyChanging" />
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    ///     Base class for MVVM VMs
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    // ReSharper disable once InconsistentNaming
    public abstract class VMBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <inheritdoc />
        /// <summary>
        ///     When a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        /// <summary>
        ///     When a property is changing
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        ///     Called before changing a property value
        /// </summary>
        /// <param name="propName">The name of the property</param>
        protected void OnPropertyChanging([NotNull] [CallerMemberName] string propName = "") {
            if (propName is null) {
                throw Internals.ExceptionGenerator.ArgumentNull(nameof(propName), nameof(OnPropertyChanging));
            }

            DelegatePropertyChanging(propName);
        }

        /// <summary>
        ///     Called after changing a property value
        /// </summary>
        /// <param name="propName">The name of the property</param>
        protected void OnPropertyChanged([NotNull] [CallerMemberName] string propName = "") {
            if (propName is null) {
                throw Internals.ExceptionGenerator.ArgumentNull(nameof(propName), nameof(OnPropertyChanged));
            }

            DelegatePropertyChanged(propName);
        }

        /// <summary>
        ///     Automate calls to <see cref="OnPropertyChanging" /> and <see cref="OnPropertyChanged" />
        /// </summary>
        /// <code>
        /// public string Property {get => _backingField; set => EditProperty(ref _backingField, value); }
        /// private string _backingField;
        /// </code>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="backingField">A ref to the backing field of the property</param>
        /// <param name="newValue">The new value of the property</param>
        /// <param name="propName">The name of the property</param>
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        protected void EditProperty<T>([CanBeNull] ref T backingField, [CanBeNull] T newValue,
            [NotNull] [CallerMemberName] string propName = "") {
            DelegatePropertyChanging(propName);
            backingField = newValue;
            DelegatePropertyChanged(propName);
        }

        /// <summary>
        ///     Delegate a call to <see cref="PropertyChanging" />
        /// </summary>
        /// <param name="propName">The property being changed</param>
        protected void DelegatePropertyChanging([CanBeNull] string propName) {
            if (string.IsNullOrWhiteSpace(propName)) {
                throw Internals.ExceptionGenerator.ArgumentNullOrEmpty( nameof(propName));
            }

            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propName));
        }

        /// <summary>
        ///     Delegate a call to <see cref="PropertyChanged" />
        /// </summary>
        /// <param name="propName">The property after the changes has been made</param>
        protected void DelegatePropertyChanged([CanBeNull] string propName) {
            if (string.IsNullOrWhiteSpace(propName)) {
                throw Internals.ExceptionGenerator.ArgumentNullOrEmpty( nameof(propName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}