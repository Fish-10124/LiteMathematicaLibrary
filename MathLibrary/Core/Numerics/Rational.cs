using System;
using System.Numerics;

namespace MathLibrary.Core.Numerics;

public sealed class Rational : NumberExpr
{
    public override bool IsZero => Numerator.IsZero;

    public override bool IsPositiveOne => Numerator == Denominator && !Numerator.IsZero;

    public override bool IsNegativeOne => Numerator == -Denominator && !Numerator.IsZero;

    public override bool IsPositive => Numerator.Sign > 0;

    public override bool IsNegative => Numerator.Sign < 0;

    public BigInteger Numerator { get; }

    public BigInteger Denominator { get; }

    public override NumberKind Kind => NumberKind.Rational;

    public Rational(BigInteger numerator, BigInteger denominator)
    {
        if (denominator == 0)
        {
            throw new DivideByZeroException();
        }

        if (denominator < 0)
        {
            numerator = -numerator;
            denominator = -denominator;
        }

        var gcd = BigInteger.GreatestCommonDivisor(BigInteger.Abs(numerator), denominator);
        if (gcd > 1)
        {
            numerator /= gcd;
            denominator /= gcd;
        }

        Numerator = numerator;
        Denominator = denominator;
    }

    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is Rational other) return this.Numerator == other.Numerator && this.Denominator == other.Denominator;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Numerator, Denominator);
    }
}
