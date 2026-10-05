using System;

namespace MathLibrary.Core.Operations;

public sealed class Power : Expr
{
    public Expr Base { get; }

    public Expr Exponent { get; }

    public Power(Expr @base, Expr exponent)
    {
        Base = @base;
        Exponent = exponent;
    }

    public override string ToString()
    {
        return $"({Base}^{Exponent})";
    }
}
