using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Utils;

public static class Converter
{
    public static Complex ToComplex(NumberExpr num)
    {
        if (num is Complex c) return c;
        return new Complex(num, new Integer(0));
    }

    public static BigReal ToBigReal(NumberExpr num)
    {
        if (num is BigReal br) return br;
        if (num is Integer i) return new BigReal(i.Value, 0, 0);
        if (num is Rational r) return new Times(r.Numerator, new Power(r.Denominator, new Integer(-1)))
    }
}
