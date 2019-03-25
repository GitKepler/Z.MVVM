﻿#region USINGS

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.MVVMHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        [TestInitialize]
        public void Init() { }

        [TestCleanup]
        public void Cleanup() { }

        [TestMethod]
        public void TestRegexValidationNotPass() {
            var vm = new FakeVM {
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
            var vm = new FakeVM {
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
            var vm = new FakeVM {
                CheckedExpression2 = "abcdefghijklmn"
            };
            Assert.IsTrue(vm.Errors.ContainsKey(nameof(vm.CheckedExpression2)));
            string error = vm.Errors[nameof(vm.CheckedExpression2)]?.FirstOrDefault();
            Assert.IsNotNull(error);
            Assert.AreEqual("String length must be < 10", error);
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void TestCustomValidationWithTextPass() {
            var vm = new FakeVM {
                CheckedExpression2 = "abcdef"
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
            var vm = new FakeVM {
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
            var vm = new FakeVM {
                CheckedExpression1 = "abcdefghijklmnop"
            };
            if (vm.Errors.ContainsKey(nameof(vm.CheckedExpression1))) {
                IReadOnlyList<string> value = vm.Errors[nameof(vm.CheckedExpression1)];
                Assert.IsNotNull(value);
                Assert.AreEqual(0, value.Count);
            }

            Assert.IsFalse(vm.HasErrors);
        }

        [TestMethod]
        public void TestNullCheckNotPass() {
            var vm = new FakeVM {
                CheckedNull = null
            };
            Assert.IsTrue(vm.Errors.ContainsKey(nameof(vm.CheckedNull)));
            string error = vm.Errors[nameof(vm.CheckedNull)]?.FirstOrDefault();
            Assert.IsNotNull(error);
            Assert.AreEqual($"The value of {nameof(vm.CheckedNull)} is null.", error);
            Assert.IsTrue(vm.HasErrors);
        }

        [TestMethod]
        public void TestNullCheckPass() {
            var vm = new FakeVM {
                CheckedNull = "test"
            };
            if (vm.Errors.ContainsKey(nameof(vm.CheckedNull))) {
                IReadOnlyList<string> value = vm.Errors[nameof(vm.CheckedNull)];
                Assert.IsNotNull(value);
                Assert.AreEqual(0, value.Count);
            }

            Assert.IsFalse(vm.HasErrors);
        }

        [TestMethod]
        public void ModifyPropertyTest() {
            var vm = new FakeVM();
            var pre = "";
            var post = "";
            var preI = 0;
            var postI = 0;
            vm.PropertyChanging += (sender, args) =>
            {
                Assert.AreEqual(nameof(vm.NotChecked), args.PropertyName);
                Interlocked.Increment(ref preI);
                pre = vm.NotChecked;
            };
            vm.PropertyChanged += (sender, args) =>
            {
                Assert.AreEqual(nameof(vm.NotChecked), args.PropertyName);
                Interlocked.Increment(ref postI);
                post = vm.NotChecked;
            };
            vm.NotChecked = "test";
            Assert.IsTrue(string.IsNullOrEmpty(pre));
            Assert.AreEqual("test", post);
            Assert.AreEqual("test", vm.NotChecked);
            Assert.AreEqual(1, preI);
            Assert.AreEqual(1, postI);
        }

        [TestMethod]
        public void ModifyPropertyNonRepetTest() {
            var vm = new FakeVM();
            var pre = "";
            var post = "";
            var preI = 0;
            var postI = 0;
            vm.PropertyChanging += (sender, args) =>
            {
                Assert.AreEqual(nameof(vm.NotChecked), args.PropertyName);
                Interlocked.Increment(ref preI);
                pre = vm.NotChecked;
            };
            vm.PropertyChanged += (sender, args) =>
            {
                Assert.AreEqual(nameof(vm.NotChecked), args.PropertyName);
                Interlocked.Increment(ref postI);
                post = vm.NotChecked;
            };
            vm.NotChecked = "test";
            Assert.IsTrue(string.IsNullOrEmpty(pre));
            Assert.AreEqual("test", post);
            Assert.AreEqual("test", vm.NotChecked);
            Assert.AreEqual(1, preI);
            Assert.AreEqual(1, postI);
            vm.NotChecked = "test";
            Assert.IsTrue(string.IsNullOrEmpty(pre));
            Assert.AreEqual("test", post);
            Assert.AreEqual("test", vm.NotChecked);
            Assert.AreEqual(1, preI);
            Assert.AreEqual(1, postI);
        }

        public class FakeVM : VmBase
        {
            private string _checkedExpression1;
            private string _checkedExpression2;
            private object _checkedNull;
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

            [NotNullValidation(nameof(CheckedNull))]
            public object CheckedNull { get => _checkedNull; set => EditProperty(ref _checkedNull, value); }
        }
    }
}