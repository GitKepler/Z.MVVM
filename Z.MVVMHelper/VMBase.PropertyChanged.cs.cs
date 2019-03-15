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
    public abstract partial class VmBase
    {
        /// <inheritdoc />
        /// <summary>
        ///     When a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Called after changing a property value
        /// </summary>
        /// <param name="propName">The name of the property</param>
        protected void OnPropertyChanged([NotNull] [CallerMemberName] string propName = "") {
            DelegatePropertyChanged(propName);
        }

        /// <summary>
        ///     Delegate a call to <see cref="PropertyChanged" />
        /// </summary>
        /// <param name="propName">The property after the changes has been made</param>
        protected void DelegatePropertyChanged([CanBeNull] string propName) {
            ValueValidator.ArgumentNull(propName, nameof(propName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}