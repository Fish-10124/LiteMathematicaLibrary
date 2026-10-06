using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core;

public static class Symbols
{
    public static readonly Symbol Indeterminate = new Symbol(nameof(Indeterminate));
    public static readonly Symbol Undefined = new Symbol(nameof(Undefined));
    public static readonly Symbol ComplexInfinity = new Symbol(nameof(ComplexInfinity));
    public static readonly Symbol Infinity = new Symbol(nameof(Infinity));
}