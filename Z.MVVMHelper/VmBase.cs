#region USINGS

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

#endregion

namespace Z.MVVMHelper
{
    public abstract class VmBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        ///     Called before changing a property value
        /// </summary>
        /// <param name="propName">The name of the property</param>
        protected void OnPropertyChanging([NotNull] [CallerMemberName] string propName = "") {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propName));
        }

        /// <summary>
        ///     Called after changing a property value
        /// </summary>
        /// <param name="propName">The name of the property</param>
        protected void OnPropertyChanged([NotNull] [CallerMemberName] string propName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
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
        protected void EditProperty<T>([CanBeNull] ref T backingField, [CanBeNull] T newValue,
            [NotNull] [CallerMemberName] string propName = "") {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propName));
            backingField = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}