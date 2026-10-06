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

    public override NumberRank Rank => NumberRank.Complex;

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

    public override NumberExpr ToNextRank()
    {
        return this;
    }

    protected override NumberExpr AddSameType(NumberExpr expr)
    {
        var same = expr as Complex ?? throw new ArgumentException("Argument is not the same type", nameof(expr));
        Expr newReal = new Plus(this.Real, same.Real).Evaluate();
        Expr newImag = new Plus(this.Imaginary, same.Imaginary).Evaluate();
        return new Complex(newReal, newImag);
    }

    protected override NumberExpr MultiplySameType(NumberExpr expr)
    {
        var same = expr as Complex ?? throw new ArgumentException("Argument is not the same type", nameof(expr));

        var a = this.Real;
        var b = this.Imaginary;
        var c = same.Real;
        var d = same.Imaginary;

        Expr ac = new Times(a, c).Evaluate();
        Expr bd = new Times(b, d).Evaluate();
        Expr negBd = new Times(new Integer(-1), bd).Evaluate();
        Expr newReal = new Plus(ac, negBd).Evaluate();

        Expr ad = new Times(a, d).Evaluate();
        Expr bc = new Times(b, c).Evaluate();
        Expr newImag = new Plus(ad, bc).Evaluate();

        return new Complex(newReal, newImag);
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
