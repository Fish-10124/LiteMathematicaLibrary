using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Functions;

public sealed class Power : Function
{
    public Power(Expr @base, Expr exponent) : base(nameof(Power), @base, exponent) { }
}

