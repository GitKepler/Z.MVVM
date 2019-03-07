using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Z.MVVMHelper.Commands
{
    public abstract class SynchronousCommandBase : CommandBase
    {
        /// <inheritdoc />
        public SynchronousCommandBase([NotNull] Predicate<object> canExecute) : base(canExecute) { }

        #region Overrides of CommandBase

        /// <inheritdoc />
        protected override void Run(object parameter, bool catchError) {
            MethodStart();
            try {
                RunSynchronously(parameter);
            } finally {
                MethodEnd();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        protected abstract void RunSynchronously([CanBeNull] object parameter);

        #endregion
    }
}
