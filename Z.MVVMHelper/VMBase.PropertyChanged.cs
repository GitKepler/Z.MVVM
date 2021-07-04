#region USINGS

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

#endregion

namespace Z.MVVMHelper
{
    public abstract partial class VmBase
    {
        /// <inheritdoc />
        /// <summary>
        ///     When a property is changed
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        ///     Called after changing a property value
        /// </summary>
        /// <param name="propName">The name of the property</param>
        protected void OnPropertyChanged([CallerMemberName] string propName = "") {
            if (propName is null) throw new ArgumentNullException(nameof(propName));

            DelegatePropertyChanged(propName);
        }

        /// <summary>
        ///     Delegate a call to <see cref="PropertyChanged" />
        /// </summary>
        /// <param name="propName">The property after the changes has been made</param>
        protected void DelegatePropertyChanged(string propName) {
            if (propName is null) throw new ArgumentNullException(nameof(propName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
