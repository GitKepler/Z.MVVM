#region USINGS

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Z.MVVMHelper
{
    /// <inheritdoc />
    [AttributeUsage(AttributeTargets.Property)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AnchorAttribute : Attribute { }
}