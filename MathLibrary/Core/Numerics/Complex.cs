using System;
using MathLibrary.Core.Functions;

namespace MathLibrary.Core.Numerics;

public sealed class Complex : NumberExpr
{

    public override bool IsZero => (Real is NumberExpr r && r.IsZero) && (Imaginary is NumberExpr i && i.IsZero);

    public override bool IsPositiveOne => (Real is NumberExpr r && r.IsPositiveOne) && (Imaginary is NumberExpr i && i.IsZero);

    public override bool IsNegativeOne => (Real is NumberExpr r && r.IsNegativeOne) && (Imaginary is NumberExpr i && i.IsZero);

    public override bool IsPositive => (Imaginary is NumberExpr i && i.IsZero) && (Real is NumberExpr r && r.IsPositive);

    public override bool IsNegative => (Imaginary is NumberExpr i && i.IsZero) && (Real is NumberExpr r && r.IsNegative);

    public Expr Real { get; }

    public Expr Imaginary { get; }

    public override NumberKind Kind => NumberKind.Complex;

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

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is Complex other) return Equals(this.Real, other.Real) && Equals(this.Imaginary, other.Imaginary);
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Real, Imaginary);
    }
}
