#region USINGS

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper
{
    public abstract partial class VmBase : INotifyPropertyChanging
    {
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
            DelegatePropertyChanging(propName);
        }

        /// <summary>
        ///     Delegate a call to <see cref="PropertyChanging" />
        /// </summary>
        /// <param name="propName">The property being changed</param>
        protected void DelegatePropertyChanging([CanBeNull] string propName) {
            ValueValidator.ArgumentNull(propName, nameof(propName));

            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propName));
        }
    }
}