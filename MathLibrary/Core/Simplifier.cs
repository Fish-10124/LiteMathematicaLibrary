using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core;

public static class Simplifier
{
    public static Expr Simplify(Expr expr)
    {
        ArgumentNullException.ThrowIfNull(expr);

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
