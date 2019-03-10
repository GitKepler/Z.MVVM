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
        [NotNull]
        private ConcurrentDictionary<string, List<string>> NotifyDataErrorInfoErrors { get; } = new ConcurrentDictionary<string, List<string>>();

        /// <inheritdoc />
        public bool HasErrors => NotifyDataErrorInfoErrors.Any(k => k.Value?.Any() ?? false);

        /// <inheritdoc />
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <inheritdoc />
        [CanBeNull]
        public IEnumerable GetErrors([NotNull] string propertyName) {
            return NotifyDataErrorInfoErrors[propertyName].AsEnumerable();
        }

        /// <summary>
        /// </summary>
        protected void InitializeDataErrorInfo() {
            PropertyChanged += NotifyDataErrorInfo_PropertyChanged;
        }

        private void NotifyDataErrorInfo_PropertyChanged([CanBeNull] object sender, [NotNull] PropertyChangedEventArgs e) {
            if (e.PropertyName is null) {
                return;
            }

            if (!ValidationAttributes.ContainsKey(e.PropertyName)) {
                return;
            }

            IReadOnlyList<string> errors = FetchErrors(e.PropertyName);


            List<string> bag = NotifyDataErrorInfoErrors[e.PropertyName];
            bool isError = errors.Any();
            switch (bag) {
                case null when isError:
                    bag = new List<string>();
                    NotifyDataErrorInfoErrors[e.PropertyName] = bag;
                    break;
                case null:
                    return;
            }

            bool containedError = bag.Any();

            bool sameExc = IsSame(bag.ToArray(), errors.ToArray());
            if (sameExc) {
                return;
            }

            bag.Clear();
            bag.AddRange(errors);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(e.PropertyName));
        }

        private bool IsSame([NotNull] string[] a, [NotNull] string[] b) {
            ValueValidator.ArgumentNull(a, nameof(a));
            ValueValidator.ArgumentNull(b, nameof(b));
            if (a.Length != b.Length) {
                return false;
            }

            IGrouping<string, string>[] groupedA = a.GroupBy(v => v).ToArray();
            IGrouping<string, string>[] groupedB = b.GroupBy(v => v).ToArray();
            return groupedA.Length == groupedB.Length && groupedA.All(grouping => groupedB.Any(g => g.Key == grouping.Key));
        }
    }
}