# Z.MVVM
Simple MVVM base library with almost 0 boilerplate code

![](https://img.shields.io/badge/.NET-Framework%204.7.2-blue.svg)

## Example of code:

*Note: Some features (marked with a \*) are not yet available in the the latest release. They are however available in the master branch.*

### VM Creation:

```cs
using Z.MVVMHelper;

namespace ProjectNamespace
{
    public class ViewModel : VmBase
    {
        // Your code here
    }
}
```
![](https://img.shields.io/badge/Boilerplate-0%25-red.svg)
![](https://img.shields.io/badge/Efficiency-100%25-brightgreen.svg)

### Synchro command creation

```cs

using Z.MVVMHelper;
using Z.MVVMHelper.Commands;

namespace ProjectNamespace
{
    public class ViewModel : VmBase
    {
        public ViewModel {
            Command = new Command(MyFunc, false);
            // OR
            Command = new Command(MyFunc, true); // to allow multiple executions
            Command = new Command(MyFunc, false, _ => true); // Default behavior, defines whether the command can be run
        }
    
        public Command Command { get; }
        
        private void MyFunction() {
            // Your code here
        }
    }
}
```

### Async command creation

```cs

using Z.MVVMHelper;
using Z.MVVMHelper.Commands;

namespace ProjectNamespace
{
    public class ViewModel : VmBase
    {
        public ViewModel {
            Command = new AsyncCommand(MyFunc, false);
            // OR
            Command = new AsyncCommand(MyFunc, true); // to allow multiple executions
            Command = new AsyncCommand(MyFunc, false, _ => true); // Default behavior, defines whether the command can be run
        }
    
        public AsyncCommand Command { get; }
        
        private async Task MyFunction() {
            // Your code here
        }
    }
}
```

Yes, it looks a lot like the previous one.

### Async command creation with cancellation capabilities

```cs

using Z.MVVMHelper;
using Z.MVVMHelper.Commands;

namespace ProjectNamespace
{
    public class ViewModel : VmBase
    {
        public ViewModel {
            Command = new AsyncCancellableCommand(MyFunc);
            // OR
            Command = new AsyncCancellableCommand(MyFunc, _ => true); // Default behavior, defines whether the command can be run
            
            CancelCommand = Command.GenerateCancelCommand(); // Generate the cancel command with all the necessary bindings to the AsyncCancellableCommand
        }
    
        public AsyncCancellableCommand Command { get; }
        public Command CancelCommand { get; }
        
        private async Task MyFunction(CancellationToken token) {
            // Your code here
            token.ThrowIfCancellationRequested(); // Is handled and will not crash your app
            // The rest of your code
        }
    }
}
```

### Easy implementation of INotifyPropertyChanged & INotifyPropertyChanging

```cs

using Z.MVVMHelper;

namespace ProjectNamespace
{
    public class ViewModel : VmBase
    {    
        public string Property { get => _property; set => EditProperty(ref _property, value); }
        private string _property;
    }
}
```

instead of

```cs

namespace ProjectNamespace
{
    public class ViewModel : INotifyPropertyChanging, INotifyPropertyChanged
    {    
        public string Property { 
            get => _property; 
            set {
                if(_property == value) {
                    return;
                }
                PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Property)));
                _property = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Property)));
            }
        }
        
        private string _property;
        
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;
    }
}
```

### Validation of input strings

```cs

using Z.MVVMHelper;
using Z.MVVMHelper.Commands;
using Z.MVVMHelper.Interfaces;
using Z.MVVMHelper.Validation;

namespace ProjectNamespace
{
    public class ViewModel : VmBase
    {
        public ViewModel {
            Command = new AsyncCommand(MyFunc, false);
            Command.BindToProperty(this, nameof(TextField)); // The command cannot be executed while the requirements are not met
        }
    
        public AsyncCommand Command { get; }
        
        [RegexValidation(nameof(TextField), @"^[^-]", AllowNull = true)]
        // It is possible to chain them (currently only boolean AND is applied)
        // Using a CustomValidator might be more effective, but a regex works too
        [ParseableValidation(nameof(TextContentValid), ParseableValidationAttribute.ParserTarget.Float)]
        public string TextField { get => _textField; set => EditProperty(ref _textField, value); } // This field must contain a positive float value
        private string _textField;
        
        private async Task MyFunction() {
            // Your code here
        }
    }
}
```

### Custom rules for input validation

```cs

using Z.MVVMHelper;
using Z.MVVMHelper.Commands;
using Z.MVVMHelper.Interfaces;
using Z.MVVMHelper.Validation;

namespace ProjectNamespace
{
    public class ViewModel : VmBase
    {
        public ViewModel {
            Command = new AsyncCommand(MyFunc, false);
            // The command cannot be executed while the requirements are not met
            Command.BindToProperty(this, nameof(TextField)); 
            // Full support when a custom ErrorGenerator is provided *
            RegisterValidator(new CustomValidator<string>(nameof(TextContentValid), text => double.TryParse(text, out double dbl) && dbl > 10, text => $"{text} is not greater than 10.")); // *
            // Limited support when the ErrorGenerator is not provided due to the use of Expression
            RegisterValidator(new CustomValidator<string>(nameof(TextContentValid), text => text.Length > 5));
            // It is also possible to use a Validation attribute (such as RegexValidationAttribute or ParseableValidationAttribute
            // since they also contains non-CLS compliant constructors which cannot be used to declare the attributes.
            // Moreover, the attributes can coexist with validators registered through RegisterValidator.
        }
    
        public AsyncCommand Command { get; }
        
        // This field must contain a double value greater than 10 and at least 5 chars
        public string TextField { get => _textField; set => EditProperty(ref _textField, value); } 
        private string _textField;
        
        private async Task MyFunction() {
            // Your code here
        }
    }
}
```

*\* The Expression is still used in the latest release*

### XAML:

#### Command usage: 

```xaml
<Button Content="ButtonText" Command="{Binding WhateverCommand}" />
```

#### Field usage example:

```xaml
<TextBox Text="{Binding WhateverField, ValidatesOnNotifyDataErrors=True}" />
```

## Build instructions:

* Check that you have the requirements to run a .NET Framework 4.7.2 application
* Open the solution in Visual Studio / whatever IDE you use that support C# with the .NET Framework
* Click build
* ...
* Profit !
