using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core;

public static class Utility
{
    public static BigInteger GetDigitLength(BigInteger value)
    {
        if (value.IsZero) return 1;
        return (BigInteger)Math.Floor(BigInteger.Log10(BigInteger.Abs(value))) + 1;
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
