using MathLibrary.Core.Numerics;
using MathLibrary.Core.Operations;
using MathLibrary.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Functions;

public sealed class Plus : Function
{
    public Plus(params Expr[] terms) : base(nameof(Plus), terms) { }

    public override Expr Simplify()
    {
        foreach (var term in Arguments)
        {
            var (coeff, @base) = Simplifier.ExtractCoefficient(term);

        }
    }

    private static NumberExpr AddNumbers(NumberExpr x, NumberExpr y)
    {
        if (x.IsZero) return y;
        if (y.IsZero) return x;

        if (x is Complex || y is Complex)
        {
            var xComp = Converter.ToComplex(x);
            var yComp = Converter.ToComplex(y);

            return new Complex(
                new Plus(xComp.Real, yComp.Real).Simplify(),
                new Plus(xComp.Imaginary, yComp.Imaginary).Simplify()
            );
        }

        if (x is BigReal || y is BigReal)
        {
            var xVal = ToBigReal(x);
            var yVal = ToBigReal(y);
        }
    }
    
}
