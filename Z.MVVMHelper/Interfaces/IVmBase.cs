using System.Collections.Generic;
using System.ComponentModel;

namespace Z.MVVMHelper.Interfaces
{
    /// <summary>
    /// Interface for VMs
    /// </summary>
    public interface IVmBase : INotifyDataErrorInfo, INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// Validation errors
        /// </summary>
        IReadOnlyDictionary<string, IReadOnlyList<string>> Errors { get; }
        /// <summary>
        /// VM-wide command repository
        /// </summary>
        ICommandRepository GlobalRepository { get; }
    }
}
