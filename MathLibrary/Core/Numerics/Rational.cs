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

        Numerator = numerator;
        Denominator = denominator;
    }

    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }
}
