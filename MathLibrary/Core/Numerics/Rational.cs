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

    public override NumberRank Rank => NumberRank.Rational;

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

    public override NumberExpr ToNextRank()
    {
        return ToBigReal(50);
    }

    public BigReal ToBigReal(BigInteger precision)
    {
        if (Numerator.IsZero)
        {
            return new BigReal(0, 0, precision);
        }

        var numLen = Utils.Utility.GetDigitLength(BigInteger.Abs(Numerator));
        var denLen = Utils.Utility.GetDigitLength(Denominator);

        BigInteger K = precision + denLen - numLen;
        if (K < 0) K = 0;

        BigInteger scaledNumerator = Numerator * BigInteger.Pow(10, (int)K);
        BigInteger mantissa = scaledNumerator / Denominator;
        BigInteger exponent = -K;

        var mantissaLen = Utils.Utility.GetDigitLength(BigInteger.Abs(mantissa));
        if (mantissaLen > precision)
        {
            BigInteger shift = mantissaLen - precision;
            mantissa /= BigInteger.Pow(10, (int)shift);
            exponent += shift;
        }

        return new BigReal(mantissa, exponent, precision);
    }

    protected override NumberExpr AddSameType(NumberExpr expr)
    {
        var same = expr as Rational ?? throw new ArgumentException("Argument is not the same type", nameof(expr));
        var newNum = this.Numerator * same.Denominator + same.Numerator * this.Denominator;
        var newDen = this.Denominator * same.Denominator;
        return new Rational(newNum, newDen);
    }

    protected override NumberExpr MultiplySameType(NumberExpr expr)
    {
        var same = expr as Rational ?? throw new ArgumentException("Argument is not the same type", nameof(expr));
        return SimplifyIfInteger(new Rational(
            this.Numerator * same.Numerator,
            this.Denominator * same.Denominator
        ));
    }

    private NumberExpr SimplifyIfInteger(Rational r)
    {
        if (r.Denominator == 1)
        {
            return new Integer(r.Numerator);
        }
        return r;
    }
}
