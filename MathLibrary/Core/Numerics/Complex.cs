using System;

namespace MathLibrary.Core.Numerics;

public sealed class Complex : NumberExpr
{

    public override bool IsZero => (Real is NumberExpr r && r.IsZero) && (Imaginary is NumberExpr i && i.IsZero);

    public override bool IsPositiveOne => (Real is NumberExpr r && r.IsPositiveOne) && (Imaginary is NumberExpr i && i.IsZero);

    public override bool IsNegativeOne => (Real is NumberExpr r && r.IsNegativeOne) && (Imaginary is NumberExpr i && i.IsZero);

    public override bool IsPositive => (Imaginary is NumberExpr i && i.IsZero) && (Real is NumberExpr r && r.IsPositive);

    public override bool IsNegative => (Imaginary is NumberExpr i && i.IsZero) && (Real is NumberExpr r && r.IsNegative);

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
