#region USINGS

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

#endregion

namespace Z.MVVMHelper.Validation
{
    /// <inheritdoc />
    public class ParseableValidationAttribute : BaseValidationAttribute
    {
        /// <summary>
        /// </summary>
        [Flags]
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
            Decimal = 3 << 8,
            Short = Int16,
            Int = Int32,
            Long = Int64,
            UShort = UInt16,
            UInt = UInt32,
            ULong = UInt64,
            Float = Single
        }

        /// <inheritdoc />
        public ParseableValidationAttribute([NotNull] string propertyName, ParserTarget target, [NotNull] CultureInfo culture) : base(propertyName) {
            switch (target) {
                case ParserTarget.Int8:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return sbyte.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Int16:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return short.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Int32:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return int.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Int64:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return long.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.UInt8:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return byte.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.UInt16:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return ushort.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.UInt32:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return uint.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.UInt64:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return ulong.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Single:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return float.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Double:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return double.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
                case ParserTarget.Decimal:
                    ErrorGenerator = o =>
                    {
                        string val = PreprocessValue<string>(o);
                        if (val is null) {
                            var s = (string) o;
                            return decimal.TryParse(s, NumberStyles.Any, culture, out _) ? string.Empty : $"Incorrect format for {target}";
                        }

                        return val;
                    };
                    break;
            }
        }

        /// <inheritdoc />
        public ParseableValidationAttribute([NotNull] string propertyName, ParserTarget target) : this(propertyName, target, CultureInfo.CurrentCulture) { }

        #region Overrides of BaseValidationAttribute

        /// <inheritdoc />
        public override Func<object, string> ErrorGenerator { get; }

        #endregion
    }
}