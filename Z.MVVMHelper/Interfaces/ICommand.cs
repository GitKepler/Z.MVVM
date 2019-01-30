using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z.MVVMHelper.Interfaces
{
    public interface ICommand : System.Windows.Input.ICommand
    {
        void Refresh();
        bool IsEnabled { get; set; }
        IExceptionHandler ExceptionHandler { get; set; }
    }
}
