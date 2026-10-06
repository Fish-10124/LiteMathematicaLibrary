using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Utils;

public static class Utility
{
    public static BigInteger GetDigitLength(BigInteger value)
    {
        if (value.IsZero) return 1;
        return (BigInteger)Math.Floor(BigInteger.Log10(BigInteger.Abs(value))) + 1;
    }
}
