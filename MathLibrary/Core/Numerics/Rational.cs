using System;
using System.Numerics;

namespace MathLibrary.Core.Numerics;

public sealed class Rational : NumberExpr
{
    public BigInteger Numerator { get; }

    public BigInteger Denominator { get; }

    public Rational(BigInteger numerator, BigInteger denominator)
    {
        if (denominator == 0)
        {
            throw new DivideByZeroException();
        }

        Numerator = numerator;
        Denominator = denominator;
    }

    public override string ToString()
    {
        return $"{Numerator}/{Denominator}";
    }
}
