#region USINGS

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Z.MVVMHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Z.MVVMHelperTests;

#endregion

namespace Z.MVVMHelper.Tests
{
    [TestClass]
    public class VmBaseTests
    {
        [NotNull] private FakeVM vm;

        [TestInitialize]
        public void Init() {
            vm = new FakeVM();
        }

        [TestCleanup]
        public void Cleanup() { }

        [TestMethod]
        public void TestRegexValidationNotPass() {
            vm.CheckedRegex = "123456";
            string error = vm.Errors[nameof(vm.CheckedRegex)];
            Assert.AreEqual($"\"{vm.CheckedRegex}\" is not a valid value for {nameof(vm.CheckedRegex)}", error);
        }

        [TestMethod]
        public void TestRegexValidationPass() {
            vm.CheckedRegex = "ABC123";
            string error = vm.Errors[nameof(vm.CheckedRegex)];
            Assert.IsTrue(string.IsNullOrWhiteSpace(error));
        }

        [TestMethod]
        public void TestCustomValidationWithTextNotPass() {
            vm.CheckedExpression1 = "abcdef";
            string error = vm.Errors[nameof(vm.CheckedExpression1)];
            Assert.AreEqual("String length must be < 10", error);
        }

        [TestMethod]
        public void TestCustomValidationWithTextPass() {
            vm.CheckedExpression1 = "abcdefghijklmn";
            string error = vm.Errors[nameof(vm.CheckedExpression1)];
            Assert.IsTrue(string.IsNullOrWhiteSpace(error));
        }
    }
}