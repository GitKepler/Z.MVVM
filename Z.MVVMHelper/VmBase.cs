#region USINGS

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Z.MVVMHelper.Interfaces;

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
    public abstract partial class VmBase : IVmBase
    {
        /// <summary>
        /// The VM-wide command repository
        /// </summary>
        
        public ICommandRepository GlobalRepository { get; }

        /// <summary>
        ///     Constructor for <see cref="VmBase" />
        /// </summary>
        protected VmBase(bool useAttributes) {
            if (useAttributes) {
                InitializeDataValidator();
            }
            InitializeINotifyDataError();
            GlobalRepository = new CommandRepository();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Constructor for <see cref="VmBase" />
        /// </summary>
        protected VmBase() : this(true) {
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
        protected void EditProperty<T>(ref T backingField, T newValue,
            [CallerMemberName] string propName = "") {
            if (backingField?.Equals(newValue) ?? false) {
                return;
            }

            DelegatePropertyChanging(propName);
            backingField = newValue;
            DelegatePropertyChanged(propName);
        }
    }
}