#region USINGS

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper
{
    /// <inheritdoc cref="INotifyPropertyChanging" />
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    ///     Base class for MVVM VMs
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    // ReSharper disable once InconsistentNaming
    public abstract class VMBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        [NotNull] private static readonly ConcurrentDictionary<Type, object> _defaultValues =
            new ConcurrentDictionary<Type, object>();

        [NotNull] private readonly ConcurrentDictionary<string, object> _backingfields =
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

        public void Initialize() {
            IEnumerable<PropertyInfo> props = GetType()
                .GetProperties()
                .Where(prop => prop.CustomAttributes?.Any(a => a?.AttributeType == typeof(AnchorAttribute)) ?? false);
            foreach (PropertyInfo propertyInfo in props) {
                if (!_defaultValues.ContainsKey(propertyInfo.PropertyType) && propertyInfo.PropertyType.IsValueType) {
                    Func<object> getDefault = Expression.Lambda<Func<object>>(
                            Expression.Convert(Expression.Default(propertyInfo.PropertyType), typeof(object)))
                        .Compile();
                    object deft = getDefault();
                    _defaultValues.TryAdd(propertyInfo.PropertyType, deft);
                } else if (propertyInfo.PropertyType.IsValueType) {
                    _defaultValues.TryAdd(propertyInfo.PropertyType, null);
                }


                _defaultValues.TryGetValue(propertyInfo.PropertyType, out object propVal);
                _backingfields.TryAdd(propertyInfo.Name, propVal);
                MethodInfo setter = propertyInfo.GetSetMethod();
                MethodInfo getter = propertyInfo.GetGetMethod();
            }
        }

        /// <summary>
        ///     Called before changing a property value
        /// </summary>
        /// <param name="propName">The name of the property</param>
        protected void OnPropertyChanging([NotNull] [CallerMemberName] string propName = "") {
            if (propName == null) {
                throw ExceptionGenerator.ArgumentNull(nameof(propName));
            }

            DelegatePropertyChanging(propName);
        }

        /// <summary>
        ///     Called after changing a property value
        /// </summary>
        /// <param name="propName">The name of the property</param>
        protected void OnPropertyChanged([NotNull] [CallerMemberName] string propName = "") {
            if (propName == null) {
                throw ExceptionGenerator.ArgumentNull(nameof(propName));
            }

            DelegatePropertyChanged(propName);
        }

        /// <summary>
        ///     Automate calls to <see cref="OnPropertyChanging" /> and <see cref="OnPropertyChanged" />
        /// </summary>
        /// <code>
        /// public string Property {get => _backingField; set => EditProperty(ref _backingField, value); }
        /// private string _backingField;
        /// </code>
        /// <typeparam name="T">The type of the property</typeparam>
        /// <param name="backingField">A ref to the backing field of the property</param>
        /// <param name="newValue">The new value of the property</param>
        /// <param name="propName">The name of the property</param>
        [SuppressMessage("ReSharper", "RedundantAssignment")]
        protected void EditProperty<T>([CanBeNull] ref T backingField, [CanBeNull] T newValue,
            [NotNull] [CallerMemberName] string propName = "") {
            DelegatePropertyChanging(propName);
            backingField = newValue;
            DelegatePropertyChanged(propName);
        }

        /// <summary>
        ///     Delegate a call to <see cref="PropertyChanging" />
        /// </summary>
        /// <param name="propName">The property being changed</param>
        protected void DelegatePropertyChanging([CanBeNull] string propName) {
            if (string.IsNullOrWhiteSpace(propName)) {
                throw ExceptionGenerator.ArgumentNullOrEmpty(nameof(propName));
            }

            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propName));
        }

        /// <summary>
        ///     Delegate a call to <see cref="PropertyChanged" />
        /// </summary>
        /// <param name="propName">The property after the changes has been made</param>
        protected void DelegatePropertyChanged([CanBeNull] string propName) {
            if (string.IsNullOrWhiteSpace(propName)) {
                throw ExceptionGenerator.ArgumentNullOrEmpty(nameof(propName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}