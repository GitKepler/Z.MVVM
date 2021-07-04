#region USINGS

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

#endregion

namespace Z.MVVMHelper
{
    public abstract partial class VmBase
    {
        private bool _hasErrors;

        /// <inheritdoc />
        public bool HasErrors { get => _hasErrors; set => EditProperty(ref _hasErrors, value); }

        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        /// <inheritdoc />
        
        public IEnumerable GetErrors(string? propertyName) {
            return FetchErrors(propertyName);
        }

        private void InitializeINotifyDataError() {
            PropertyChanged += (sender, args) =>
            {
                if (args?.PropertyName is null || !ValidationAttributes.ContainsKey(args.PropertyName)) {
                    return;
                }

                var errors = FetchErrors(args.PropertyName);
                lock (ValidationErrors) {
                    ValidationErrors.AddOrUpdate(args.PropertyName, errors, (a, b) => errors);
                    if ((errors?.Count ?? 0) == 0) {
                        ValidationErrors.TryRemove(args.PropertyName, out _);
                    }

                    var err = Errors.Values.Where(k => !(k is null)).ToArray();
                    HasErrors = err.Length > 0 && err.Any(e => (e?.Count ?? 0) > 0);
                }

                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(args.PropertyName));
            };
        }
    }
}