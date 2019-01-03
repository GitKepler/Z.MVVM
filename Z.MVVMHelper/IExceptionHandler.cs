#region USINGS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Z.MVVMHelper
{
    public interface IExceptionHandler
    {
        void HandleException<TExc>(TExc exception) where TExc : Exception;
    }
}