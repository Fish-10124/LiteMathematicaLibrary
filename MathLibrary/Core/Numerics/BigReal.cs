using System;
using System.Numerics;

namespace MathLibrary.Core.Numerics;

public sealed class BigReal : Real
{
    public BigInteger Mantissa { get; }

    public BigInteger Exponent { get; }

    public BigInteger Precision { get; }

    public override NumberRank Rank => NumberRank.Real;

    public BigReal(BigInteger mantissa, BigInteger exponent, BigInteger? precision = null)
    {
        BigInteger effectivePrecision = precision ?? CalculatePrecision(mantissa);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(effectivePrecision);

        Mantissa = mantissa;
        Exponent = exponent;
        Precision = effectivePrecision;
    }

    private static BigInteger CalculatePrecision(BigInteger mantissa)
    {
        if (mantissa.IsZero) return BigInteger.One;

        BigInteger abs = BigInteger.Abs(mantissa);
        return Utility.GetDigitLength(abs);
    }

    public override string ToString()
    {
        return $"{Mantissa}e{(Exponent >= 0 ? "+" : "")}{Exponent}";
    }

    public override NumberExpr ToNextRank()
    {
        return new Complex(this, new BigReal(0, 0, Precision));
    }

    protected override NumberExpr AddSameType(NumberExpr expr)
    {
        var same = expr as BigReal ?? throw new ArgumentException("Argument is not the same type", nameof(expr));

        // 0 的快捷处理
        if (this.Mantissa.IsZero) return same;
        if (same.Mantissa.IsZero) return this;

        BigInteger targetPrecision = BigInteger.Max(this.Precision, same.Precision);

        BigInteger m1 = this.Mantissa;
        BigInteger m2 = same.Mantissa;
        BigInteger e1 = this.Exponent;
        BigInteger e2 = same.Exponent;
        BigInteger baseExponent;

        if (e1 > e2)
        {
            BigInteger diff = e1 - e2;
            m1 *= BigInteger.Pow(10, (int)diff);
            baseExponent = e2;
        }
        else if (e2 > e1)
        {
            BigInteger diff = e2 - e1;
            m2 *= BigInteger.Pow(10, (int)diff);
            baseExponent = e1;
        }
        else
        {
            baseExponent = e1;
        }

        BigInteger sumMantissa = m1 + m2;
        if (sumMantissa.IsZero)
        {
            return new BigReal(0, 0, targetPrecision);
        }

        var currentLength = Utility.GetDigitLength(BigInteger.Abs(sumMantissa));
        BigInteger finalMantissa = sumMantissa;
        BigInteger finalExponent = baseExponent;

        if (currentLength > targetPrecision)
        {
            BigInteger shift = currentLength - targetPrecision;
            finalMantissa /= BigInteger.Pow(10, (int)shift);
            finalExponent += shift;
        }

        return new BigReal(finalMantissa, finalExponent, targetPrecision);
    }

    protected override NumberExpr MultiplySameType(NumberExpr expr)
    {
        var same = expr as BigReal ?? throw new ArgumentException("Argument is not the same type", nameof(expr));
        BigInteger targetPrecision = BigInteger.Max(this.Precision, same.Precision);
        if (this.Mantissa.IsZero || same.Mantissa.IsZero)
        {
            return new BigReal(0, 0, targetPrecision);
        }

        BigInteger prodMantissa = this.Mantissa * same.Mantissa;
        BigInteger prodExponent = this.Exponent + same.Exponent;

        var currentLength = Utility.GetDigitLength(BigInteger.Abs(prodMantissa));
        BigInteger finalMantissa = prodMantissa;
        BigInteger finalExponent = prodExponent;

        if (currentLength > targetPrecision)
        {
            BigInteger shift = currentLength - targetPrecision;
            finalMantissa /= BigInteger.Pow(10, (int)shift);
            finalExponent += shift;
        }

        return new BigReal(finalMantissa, finalExponent, targetPrecision);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is BigReal other)
        {
            return this.Mantissa == other.Mantissa
                && this.Exponent == other.Exponent
                && this.Precision == other.Precision;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Mantissa, Exponent, Precision);
    }
}
