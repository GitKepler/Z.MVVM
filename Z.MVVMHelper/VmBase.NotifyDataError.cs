#region USINGS

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Interfaces;
using Z.MVVMHelper.Internals;

#endregion

namespace Z.MVVMHelper
{
    public abstract partial class VmBase : INotifyDataErrorInfo
    {
        private bool _hasErrors;

        /// <inheritdoc />
        public bool HasErrors { get => _hasErrors; set => EditProperty(ref _hasErrors, value); }

        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <inheritdoc />
        [NotNull]
        public IEnumerable GetErrors([NotNull] string propertyName) {
            return FetchErrors(propertyName);
        }

        private void InitializeINotifyDataError() {
            PropertyChanged += (sender, args) =>
            {
                if (args?.PropertyName is null || !ValidationAttributes.ContainsKey(args.PropertyName)) {
                    return;
                }

                IReadOnlyList<string> errors = FetchErrors(args.PropertyName);
                ValidationErrors.AddOrUpdate(args.PropertyName, errors, (a, b) => errors);
                if (errors.Count <= 0) {
                    if (ValidationErrors.Values.All(v => !v?.Any() ?? true)) {
                        HasErrors = false;
                    }

                    return;
                }

                HasErrors = true;
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(args.PropertyName));
            };
        }
    }
}