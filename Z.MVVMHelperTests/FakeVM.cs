using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.MVVMHelper;
using Z.MVVMHelper.Commands;
using Z.MVVMHelper.Validation;

namespace Z.MVVMHelperTests
{
    public class FakeVM : VmBase
    {
        public string NotChecked { get => _notChecked; set => EditProperty(ref _notChecked, value); }
        private string _notChecked;

        [RegexValidation(nameof(CheckedRegex), "^ABC.*$", AllowNull = false)]
        public string CheckedRegex { get => _checkedRegex; set => EditProperty(ref _checkedRegex, value); }
        private string _checkedRegex;

        public string CheckedExpression1 { get => _checkedExpression1; set => EditProperty(ref _checkedExpression1, value); }
        private string _checkedExpression1;

        public string CheckedExpression2 { get => _checkedExpression2; set => EditProperty(ref _checkedExpression2, value); }
        private string _checkedExpression2;

        public FakeVM() {
            RegisterValidator(new CustomValidator<string>(nameof(CheckedExpression1), s => s.Length > 10));
            RegisterValidator(new CustomValidator<string>(nameof(CheckedExpression2), s => s.Length < 10, _ => $"String length must be < 10"));
        }

        public Command Command { get; }
    }
}
