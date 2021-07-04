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
        ///     When a property is changing
        /// </summary>
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <summary>
        ///     Called before changing a property value
        /// </summary>
        /// <param name="propName">The name of the property</param>
        protected void OnPropertyChanging([CallerMemberName] string propName = "") {
            if (propName is null) throw new ArgumentNullException(nameof(propName));

            DelegatePropertyChanging(propName);
        }

        /// <summary>
        ///     Delegate a call to <see cref="PropertyChanging" />
        /// </summary>
        /// <param name="propName">The property being changed</param>
        protected void DelegatePropertyChanging(string propName) {
            if (propName is null) throw new ArgumentNullException(nameof(propName));

            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propName));
        }
    }
}