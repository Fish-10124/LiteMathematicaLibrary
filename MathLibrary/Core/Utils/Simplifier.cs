using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Operations;
using MathLibrary.Core.Symbols;

namespace MathLibrary.Core.Utils;

public static class Simplifier
{
    public static Expr Simplify(Expr expr)
    {
        if (expr == null) throw new ArgumentNullException(nameof(expr));

        Expr current = expr;
        int iteration = 0;

        while (iteration < 100)
        {
            Expr next = current.Evaluate();

            if (ReferenceEquals(current, next) || current.Equals(next))
            {
                return next;
            }

            current = next;
            iteration++;
        }

        return current;
    }
}
