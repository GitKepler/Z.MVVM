#region USINGS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

#endregion

namespace Z.MVVMHelper.Internals
{
    /// <summary>
    ///     Check values and throw based on context
    /// </summary>
    internal static class ValueValidator
    {
        /// <summary>
        ///     Throws when <paramref name="obj" /> is null
        /// </summary>
        /// <typeparam name="T">Type of the param</typeparam>
        /// <param name="obj">Value to check</param>
        /// <param name="argumentName">Name of the value</param>
        /// <param name="caller">Caller</param>
        public static void ArgumentNull<T>([CanBeNull] T obj, [NotNull] string argumentName, [NotNull] [CallerMemberName] string caller = "") where T : class {
            ArgumentNullSingleCheck(argumentName, nameof(argumentName));
            ArgumentNullSingleCheck(caller, nameof(caller));
            if (obj is null) {
                throw ExceptionGenerator.ArgumentNull(argumentName, caller);
            }
        }

        private static void ArgumentNullSingleCheck<T>([NotNull] T obj, [NotNull] string argumentName, [NotNull] string caller = "") where T : class {
            if (obj is null) {
                throw ExceptionGenerator.ArgumentNull(argumentName, caller);
            }
        }

        public static void ArgumentType<TExpected, T>([CanBeNull] T obj, [NotNull] string argumentName, [NotNull] [CallerMemberName] string caller = "") {
            ArgumentNull(argumentName, nameof(argumentName));
            ArgumentNull(caller, nameof(caller));
            if (obj is TExpected) {
                return;
            }

            throw ExceptionGenerator.InvalidArgumentType<TExpected>(obj?.GetType(), argumentName);
        }
    }
}