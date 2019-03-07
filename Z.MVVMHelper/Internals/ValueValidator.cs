using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Z.MVVMHelper.Internals
{
    public static class ValueValidator
    {
        public static void ArgumentNull<T>([CanBeNull] T obj, [NotNull] string argumentName, [NotNull] [CallerMemberName] string caller = "") where T : class {
            ArgumentNull(argumentName, nameof(argumentName));
            ArgumentNull(caller, nameof(caller));
            if (obj is null) {
                throw ExceptionGenerator.ArgumentNull(argumentName, caller);
            }
        }
    }
}
