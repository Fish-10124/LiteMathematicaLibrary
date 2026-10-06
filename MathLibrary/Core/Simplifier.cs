using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core;

public static class Simplifier
{
    public static Expr Simplify(Expr expr, int maxIteration = 100)
    {
        ArgumentNullException.ThrowIfNull(expr);

        Expr current = expr;
        int iteration = 0;

        while (iteration < maxIteration)
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

    public static Expr Expand(Expr expr, int maxIteration = 100)
    {
        ArgumentNullException.ThrowIfNull(expr);

        Expr current = expr;
        int iteration = 0;

        while(iteration < maxIteration)
        {
            Expr next = current.Expand();

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
