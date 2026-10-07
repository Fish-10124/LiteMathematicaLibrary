using System;
using System.Numerics;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core;

public static class Utility
{
    public static BigInteger GetDigitLength(BigInteger value)
    {
        if (value.IsZero) return 1;
        return (BigInteger)Math.Floor(BigInteger.Log10(BigInteger.Abs(value))) + 1;
    }

    public static BigInteger Pow(BigInteger baseValue, BigInteger exponent)
    {
        if (exponent < 0) throw new ArgumentOutOfRangeException(nameof(exponent), "Exponent cannot be negative for integer power.");
        if (exponent.IsZero) return BigInteger.One;
        if (baseValue.IsZero) return BigInteger.Zero;
        if (baseValue .IsOne) return BigInteger.One;
        if (baseValue == -1) return exponent.IsEven ? BigInteger.One : -1;
        if (exponent <= int.MaxValue) return BigInteger.Pow(baseValue, (int)exponent);

        var result = BigInteger.One;
        var currentBase = baseValue;
        var exp = exponent;

        while (exp > 0)
        {
            if (!exp.IsEven) result *= currentBase;
            currentBase *= currentBase;
            exp >>= 1;
        }
        return result;
    }

    public static bool IsPositiveOne(Expr expr)
    {
        return expr is NumberExpr num && num.IsPositiveOne;
    }

    public static bool IsNegativeOne(Expr expr)
    {
        return expr is NumberExpr num && num.IsNegativeOne;
    }

    public static bool IsZero(Expr expr)
    {
        return expr is NumberExpr num && num.IsZero;
    }

    public static bool IsPositive(Expr expr)
    {
        return expr is NumberExpr num && num.IsPositive;
    }

    public static bool IsNegative(Expr expr)
    {
        return expr is NumberExpr num && num.IsNegative;
    }
}
