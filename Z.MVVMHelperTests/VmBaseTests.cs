#region USINGS

using Z.MVVMHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

using Z.MVVMHelper.Commands;
using Z.MVVMHelper.Validation;

#endregion

namespace Z.MVVMHelper.Tests
{
    public class VmBaseTests
    {
        [Fact]
        public void TestRegexValidationNotPass()
        {
            var vm = new FakeVM
            {
                CheckedRegex = "123456"
            };
            Assert.True(vm.Errors.ContainsKey(nameof(vm.CheckedRegex)));
            string error = vm.Errors[nameof(vm.CheckedRegex)]?.FirstOrDefault();
            Assert.NotNull(error);
            Assert.Equal($"\"{vm.CheckedRegex}\" is not a valid value for {nameof(vm.CheckedRegex)}", error);
            Assert.True(vm.HasErrors);
        }

        [Fact]
        public void TestRegexValidationPass()
        {
            var vm = new FakeVM
            {
                CheckedRegex = "ABC123"
            };
            if (vm.Errors.ContainsKey(nameof(vm.CheckedRegex)))
            {
                IReadOnlyList<string> value = vm.Errors[nameof(vm.CheckedRegex)];
                Assert.NotNull(value);
                Assert.Equal(0, value.Count);
            }

            Assert.False(vm.HasErrors);
        }

        [Fact]
        public void TestCustomValidationWithTextNotPass()
        {
            var vm = new FakeVM
            {
                CheckedExpression2 = "abcdefghijklmn"
            };
            Assert.True(vm.Errors.ContainsKey(nameof(vm.CheckedExpression2)));
            string error = vm.Errors[nameof(vm.CheckedExpression2)]?.FirstOrDefault();
            Assert.NotNull(error);
            Assert.Equal("String length must be < 10", error);
            Assert.True(vm.HasErrors);
        }

        [Fact]
        public void TestCustomValidationWithTextPass()
        {
            var vm = new FakeVM
            {
                CheckedExpression2 = "abcdef"
            };
            if (vm.Errors.ContainsKey(nameof(vm.CheckedExpression2)))
            {
                IReadOnlyList<string> value = vm.Errors[nameof(vm.CheckedExpression2)];
                Assert.NotNull(value);
                Assert.Equal(0, value.Count);
            }

            Assert.False(vm.HasErrors);
        }

        [Fact]
        public void TestCustomValidationWithoutTextNotPass()
        {
            var vm = new FakeVM
            {
                CheckedExpression1 = "abcdef"
            };
            Assert.True(vm.Errors.ContainsKey(nameof(vm.CheckedExpression1)));
            string error = vm.Errors[nameof(vm.CheckedExpression1)]?.FirstOrDefault();
            Assert.NotNull(error);
            Assert.Equal("s => (s.Length > 10) did not pass.", error);
            Assert.True(vm.HasErrors);
        }

        [Fact]
        public void TestCustomValidationWithoutTextPass()
        {
            var vm = new FakeVM
            {
                CheckedExpression1 = "abcdefghijklmnop"
            };
            if (vm.Errors.ContainsKey(nameof(vm.CheckedExpression1)))
            {
                IReadOnlyList<string> value = vm.Errors[nameof(vm.CheckedExpression1)];
                Assert.NotNull(value);
                Assert.Equal(0, value.Count);
            }

            Assert.False(vm.HasErrors);
        }

        [Fact]
        public void TestNullCheckNotPass()
        {
            var vm = new FakeVM
            {
                CheckedNull = null
            };
            Assert.True(vm.Errors.ContainsKey(nameof(vm.CheckedNull)));
            string error = vm.Errors[nameof(vm.CheckedNull)]?.FirstOrDefault();
            Assert.NotNull(error);
            Assert.Equal($"The value of {nameof(vm.CheckedNull)} is null.", error);
            Assert.True(vm.HasErrors);
        }

        [Fact]
        public void TestNullCheckPass()
        {
            var vm = new FakeVM
            {
                CheckedNull = "test"
            };
            if (vm.Errors.ContainsKey(nameof(vm.CheckedNull)))
            {
                IReadOnlyList<string> value = vm.Errors[nameof(vm.CheckedNull)];
                Assert.NotNull(value);
                Assert.Equal(0, value.Count);
            }

            Assert.False(vm.HasErrors);
        }

        [Fact]
        public void ModifyPropertyTest()
        {
            var vm = new FakeVM();
            var pre = "";
            var post = "";
            var preI = 0;
            var postI = 0;
            vm.PropertyChanging += (sender, args) =>
            {
                Assert.Equal(nameof(vm.NotChecked), args.PropertyName);
                Interlocked.Increment(ref preI);
                pre = vm.NotChecked;
            };
            vm.PropertyChanged += (sender, args) =>
            {
                Assert.Equal(nameof(vm.NotChecked), args.PropertyName);
                Interlocked.Increment(ref postI);
                post = vm.NotChecked;
            };
            vm.NotChecked = "test";
            Assert.True(string.IsNullOrEmpty(pre));
            Assert.Equal("test", post);
            Assert.Equal("test", vm.NotChecked);
            Assert.Equal(1, preI);
            Assert.Equal(1, postI);
        }

        [Fact]
        public void ModifyPropertyNonRepetTest()
        {
            var vm = new FakeVM();
            var pre = "";
            var post = "";
            var preI = 0;
            var postI = 0;
            vm.PropertyChanging += (sender, args) =>
            {
                Assert.Equal(nameof(vm.NotChecked), args.PropertyName);
                Interlocked.Increment(ref preI);
                pre = vm.NotChecked;
            };
            vm.PropertyChanged += (sender, args) =>
            {
                Assert.Equal(nameof(vm.NotChecked), args.PropertyName);
                Interlocked.Increment(ref postI);
                post = vm.NotChecked;
            };
            vm.NotChecked = "test";
            Assert.True(string.IsNullOrEmpty(pre));
            Assert.Equal("test", post);
            Assert.Equal("test", vm.NotChecked);
            Assert.Equal(1, preI);
            Assert.Equal(1, postI);
            vm.NotChecked = "test";
            Assert.True(string.IsNullOrEmpty(pre));
            Assert.Equal("test", post);
            Assert.Equal("test", vm.NotChecked);
            Assert.Equal(1, preI);
            Assert.Equal(1, postI);
        }

        public class FakeVM : VmBase
        {
            private string _checkedExpression1;
            private string _checkedExpression2;
            private object _checkedNull;
            private string _checkedRegex;
            private string _notChecked;

            public FakeVM()
            {
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