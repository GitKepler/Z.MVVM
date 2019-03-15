#region USINGS

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Z.MVVMHelper.Internals;

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
    public abstract partial class VmBase
    {
        /// <summary>
        ///     Constructor for <see cref="VmBase" />
        /// </summary>
        protected VmBase(bool useAttributes) {
            if (useAttributes) {
                InitializeDataValidator();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Constructor for <see cref="VmBase" />
        /// </summary>
        protected VmBase() : this(true) {
            InitializeINotifyDataError();
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
            if (backingField?.Equals(newValue) ?? false) {
                return;
            }

            DelegatePropertyChanging(propName);
            backingField = newValue;
            DelegatePropertyChanged(propName);
        }
    }
}