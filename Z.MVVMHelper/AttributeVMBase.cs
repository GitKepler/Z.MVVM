#region USINGS

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper
{
    /// <summary>
    ///     Base class for MVVM VMs using attributes instead of backing fields
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AttributeVMBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        [NotNull] private readonly ConcurrentDictionary<string, object> _backingFields =
            new ConcurrentDictionary<string, object>();

        /// <inheritdoc />
        /// <summary>
        ///     When a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc />
        /// <summary>
        ///     When a property is changing
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        protected void Initialize() {
            IEnumerable<PropertyInfo> props = GetType()
                .GetProperties()
                .Where(prop => prop.CustomAttributes?.Any(a => a?.AttributeType == typeof(AnchorAttribute)) ?? false);
            foreach (PropertyInfo propertyInfo in props) {
                MethodInfo setter = propertyInfo.GetSetMethod();
                MethodInfo getter = propertyInfo.GetGetMethod();
            }
        }

        /// <summary>
        ///     Internal call to get the value of a property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propName"></param>
        /// <returns></returns>
        [CanBeNull]
        protected TProperty GetProperty<TProperty>([NotNull] [CallerMemberName] string propName = "") {
            if (string.IsNullOrWhiteSpace(propName)) {
                throw ExceptionGenerator.ArgumentNullOrEmpty(nameof(propName));
            }

            return (TProperty) _backingFields.GetOrAdd(propName, _ => default(TProperty));
        }

        /// <summary>
        ///     Internal call to set the value of a property
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="propName"></param>
        /// <param name="value"></param>
        protected void SetProperty<TProperty>([CanBeNull] TProperty value,
            [NotNull] [CallerMemberName] string propName = "") {
            if (string.IsNullOrWhiteSpace(propName)) {
                throw ExceptionGenerator.ArgumentNullOrEmpty(nameof(propName));
            }

            _backingFields.AddOrUpdate(propName, value, (_1, _2) => value);
        }

        [NotNull]
        private static MethodInfo GenerateGetter([NotNull] MethodInfo getter, [NotNull] string propName,
            [NotNull] Type propType, [NotNull] Type objectType, [NotNull] MethodInfo getterOverride) {
            var dynamicMethod = new DynamicMethod($"get_{propName}", propType, null, objectType.Module);
            ILGenerator generator = dynamicMethod.GetILGenerator(256);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldstr, propName);
            generator.Emit(OpCodes.Call, getterOverride); // Specialized version
            generator.Emit(OpCodes.Ret);
            // Need to create the delegate
            throw ExceptionGenerator.Todo("Need to create another XAnchorAttribute for ref/values");
        }

        [NotNull]
        private static MethodInfo GenerateSetter([NotNull] MethodInfo setter, [NotNull] string propName,
            [NotNull] Type propType, [NotNull] Type objectType, [NotNull] MethodInfo setterOverride) {
            var dynamicMethod = new DynamicMethod($"set_{propName}", null, new[] {propType}, objectType.Module);
            ILGenerator generator = dynamicMethod.GetILGenerator(256);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldstr, propName);
            generator.Emit(OpCodes.Call, setterOverride); // Specialized version
            generator.Emit(OpCodes.Ret);
            // Need to create the delegate
            throw ExceptionGenerator.Todo("Need to create another XAnchorAttribute for ref/values");
        }
    }
}