using System;
using System.Numerics;

namespace MathLibrary.Core.Numerics;

public sealed class BigReal : Real
{
    public BigInteger Mantissa { get; }

    public BigInteger Exponent { get; }

    public BigInteger Precision { get; }

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

    public NumberExpr MultiplySameType(NumberExpr expr)
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
}
