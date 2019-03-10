#region USINGS

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Z.MVVMHelper
{
    public abstract partial class VmBase : INotifyDataErrorInfo
    {
        public bool HasErrors => !IsValid;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName) {
            throw new NotImplementedException();
        }
    }
}