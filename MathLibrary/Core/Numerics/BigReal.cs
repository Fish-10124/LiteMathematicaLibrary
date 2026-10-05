using System;
using System.Numerics;

namespace MathLibrary.Core.Numerics;

public sealed class BigReal : Real
{
    public BigInteger Mantissa { get; }

    public BigInteger Exponent { get; }

    public BigInteger Precision { get; }

    public BigReal(BigInteger mantissa, BigInteger exponent, BigInteger precision)
    {
        if (precision <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(precision));
        }

        Mantissa = mantissa;
        Exponent = exponent;
        Precision = precision;
    }

    public override string ToString()
    {
        return $"{Mantissa}e{(Exponent >= 0 ? "+" : "")}{Exponent}";
    }
}
