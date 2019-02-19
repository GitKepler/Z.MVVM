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
    ///     Generate exception based on context
    /// </summary>
    internal static class ExceptionGenerator
    {
        /// <summary>
        ///     Generate an exception when an object of the wrong type is passed as an argument of a function
        /// </summary>
        /// <typeparam name="TExpected"></typeparam>
        /// <param name="received"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [NotNull]
        public static ArgumentException InvalidArgumentType<TExpected>([CanBeNull] Type received,
            [CanBeNull] string param) {
            if (param == null && received is null) {
                return new ArgumentException(
                    $"The expected type ({nameof(TExpected)}) does not match the received type");
            }

            if (param == null) {
                return new ArgumentException(
                    $"The expected type ({nameof(TExpected)}) does not match the received type ({received.Name})");
            }

            if (received is null) {
                return new ArgumentException(
                    $"The expected type ({nameof(TExpected)}) does not match the received type",
                    param);
            }

            return new ArgumentException(
                $"The expected type ({nameof(TExpected)}) does not match the received type ({received.Name})",
                param);
        }

        /// <summary>
        ///     Generate an exception when a parameter has a null value
        /// </summary>
        /// <param name="param"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        [NotNull]
        public static ArgumentNullException ArgumentNull([NotNull] string param,
            [NotNull] [CallerMemberName] string function = "") {
            if (param is null) {
                throw ArgumentNull(nameof(param));
            }

            return new ArgumentNullException(
                param,
                $"To call {function} you need to provide a non-null value for {param}");
        }

        /// <summary>
        /// Generate an exception when a parameter is null or empty
        /// </summary>
        /// <param name="param"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        [NotNull]
        public static ArgumentException ArgumentNullOrEmpty([CanBeNull] string param,
            [NotNull] [CallerMemberName] string function = "") {
            if (param is null) {
                return new ArgumentException("Value cannot be null or whitespace.");
            }

            return new ArgumentException("Value cannot be null or whitespace.", param);
        }
    }
}