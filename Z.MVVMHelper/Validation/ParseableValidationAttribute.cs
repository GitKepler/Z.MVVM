#region USINGS

using System;
using System.Globalization;

#endregion

namespace Z.MVVMHelper.Validation
{
    /// <inheritdoc />
    public sealed class ParseableValidationAttribute : BaseValidationAttribute
    {
        /// <summary>
        /// </summary>
        public enum ParserTarget
        {
            Int8 = SByte,
            UInt8 = Byte,
            Int16 = (1 << 8) + 16,
            Int32 = (1 << 8) + 32,
            Int64 = (1 << 8) + 64,
            UInt16 = 16,
            UInt32 = 32,
            UInt64 = 64,
            Byte = 8,
            SByte = (1 << 8) + 8,
            Double = (3 << 8) + 64,
            Single = (3 << 8) + 32,
            Decimal = (3 << 8) + 128,
            Short = Int16,
            Int = Int32,
            Long = Int64,
            UShort = UInt16,
            UInt = UInt32,
            ULong = UInt64,
            Float = Single
        }

        /// <inheritdoc />
        public ParseableValidationAttribute(string propertyName, ParserTarget target, IFormatProvider culture) : base(propertyName) {
            switch (target) {
                case ParserTarget.Int8:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : sbyte.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Int16:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : short.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Int32:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : int.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Int64:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : long.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.UInt8:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : byte.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.UInt16:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : ushort.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.UInt32:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : uint.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.UInt64:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : ulong.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Single:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : float.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Double:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : double.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Decimal:
                    ErrorGenerator = o =>
                    {
                        var val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = o as string;
                            return o is null ? string.Empty : decimal.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target));
            }
        }

        /// <inheritdoc />
        public ParseableValidationAttribute(string propertyName, ParserTarget target) : this(propertyName, target, CultureInfo.CurrentCulture) { }

        #region Overrides of BaseValidationAttribute

        /// <inheritdoc />
        public override Func<object?, string> ErrorGenerator { get; }

        #endregion
    }
}