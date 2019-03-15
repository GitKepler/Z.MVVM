#region USINGS

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.MVVMHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelper.Commands;
using Z.MVVMHelper.Validation;

#endregion

namespace Z.MVVMHelper.Tests
{
    [TestClass]
    public class VmBaseTests
    {
        [NotNull] private FakeVM vm;

        [TestInitialize]
        public void Init() { }

        [TestCleanup]
        public void Cleanup() { }

        [TestMethod]
        public void TestRegexValidationNotPass() {
            vm = new FakeVM {
                CheckedRegex = "123456"
            };
            Assert.IsTrue(vm.Errors.ContainsKey(nameof(vm.CheckedRegex)));
            string error = vm.Errors[nameof(vm.CheckedRegex)]?.FirstOrDefault();
            Assert.IsNotNull(error);
            Assert.AreEqual($"\"{vm.CheckedRegex}\" is not a valid value for {nameof(vm.CheckedRegex)}", error);
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void TestRegexValidationPass() {
            vm = new FakeVM {
                CheckedRegex = "ABC123"
            };
            if (vm.Errors.ContainsKey(nameof(vm.CheckedRegex))) {
                IReadOnlyList<string> value = vm.Errors[nameof(vm.CheckedRegex)];
                Assert.IsNotNull(value);
                Assert.AreEqual(0, value.Count);
            }

            Assert.IsFalse(vm.HasErrors);
        }

        [TestMethod]
        public void TestCustomValidationWithTextNotPass() {
            vm = new FakeVM {
                CheckedExpression2 = "abcdef"
            };
            Assert.IsTrue(vm.Errors.ContainsKey(nameof(vm.CheckedExpression2)));
            string error = vm.Errors[nameof(vm.CheckedExpression2)]?.FirstOrDefault();
            Assert.IsNotNull(error);
            Assert.AreEqual("String length must be < 10", error);
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void TestCustomValidationWithTextPass() {
            vm = new FakeVM {
                CheckedExpression2 = "abcdefghijklmn"
            };
            if (vm.Errors.ContainsKey(nameof(vm.CheckedExpression2))) {
                IReadOnlyList<string> value = vm.Errors[nameof(vm.CheckedExpression2)];
                Assert.IsNotNull(value);
                Assert.AreEqual(0, value.Count);
            }

            Assert.IsFalse(vm.HasErrors);
        }

        [TestMethod]
        public void TestCustomValidationWithoutTextNotPass() {
            vm = new FakeVM {
                CheckedExpression1 = "abcdef"
            };
            Assert.IsTrue(vm.Errors.ContainsKey(nameof(vm.CheckedExpression1)));
            string error = vm.Errors[nameof(vm.CheckedExpression1)]?.FirstOrDefault();
            Assert.IsNotNull(error);
            Assert.AreEqual("s => (s.Length > 10) did not pass.", error);
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void TestCustomValidationWithoutTextPass() {
            vm = new FakeVM {
                CheckedExpression1 = "abcdefghijklmnop"
            };
            if (vm.Errors.ContainsKey(nameof(vm.CheckedExpression1))) {
                IReadOnlyList<string> value = vm.Errors[nameof(vm.CheckedExpression1)];
                Assert.IsNotNull(value);
                Assert.AreEqual(0, value.Count);
            }

            Assert.IsFalse(vm.HasErrors);
        }

        public class FakeVM : VmBase
        {
            private string _checkedExpression1;
            private string _checkedExpression2;
            private string _checkedRegex;
            private string _notChecked;

            public FakeVM() {
                RegisterValidator(new CustomValidator<string>(nameof(CheckedExpression1), s => s.Length > 10));
                RegisterValidator(new CustomValidator<string>(nameof(CheckedExpression2), s => s.Length < 10, _ => "String length must be < 10"));
            }

            public string NotChecked { get => _notChecked; set => EditProperty(ref _notChecked, value); }

            [RegexValidation(nameof(CheckedRegex), "^ABC.*$", AllowNull = false)]
            public string CheckedRegex { get => _checkedRegex; set => EditProperty(ref _checkedRegex, value); }

            public string CheckedExpression1 { get => _checkedExpression1; set => EditProperty(ref _checkedExpression1, value); }

            public string CheckedExpression2 { get => _checkedExpression2; set => EditProperty(ref _checkedExpression2, value); }

            public Command Command { get; }
        }
    }
}