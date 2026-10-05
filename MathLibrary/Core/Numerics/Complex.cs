using System;

namespace MathLibrary.Core.Numerics;

public sealed class Complex : NumberExpr
{
    private Expr Real { get; }

    private Expr Imaginary { get; }

    public Complex(Expr real, Expr imaginary)
    {
        Real = real;
        Imaginary = imaginary;
    }

    public override string ToString()
    {
        // TODO: 先这么写
        return $"({Real}+{Imaginary}i)";
    }
}
