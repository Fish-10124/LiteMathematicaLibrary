using System;
using System.Numerics;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Operations;
using MathLibrary.Core.Symbols;

namespace MathLibrary.Core.Utils;

public static class Simplifier
{
    public static Expr Simplify(Expr expr)
    {
        
    }

    private static Expr SimplifyAdd(Add expr)
    {
        var left = Simplify(expr.Left);
        var right = Simplify(expr.Right);

        if (right is NumberExpr r && r.IsZero)
        {
            return left;
        }

        if (left is NumberExpr l && l.IsZero)
        {
            return right;
        }

        if (left is Integer lInt && right is Integer rInt)
        {
            return new Integer(lInt.Value + rInt.Value);
        }

        if (ReferenceEquals(left, expr.Left) && ReferenceEquals(right, expr.Right))
        {
            return expr;
        }

        return new Add(left, right);
    }

    private static Expr SimplifyMultiply(Multiply expr)
    {
        var left = Simplify(expr.Left);
        var right = Simplify(expr.Right);

        if (left is NumberExpr l)
        {
            if (l.IsZero) return new Integer(0);
            if (l.IsPositive) return right;
        }

        if (right is NumberExpr r)
        {
            if (r.IsZero) return new Integer(0);
            if (r.IsPositiveOne) return left;
        }

        if (left is Integer lInt && right is Integer rInt)
        {
            return new Integer(lInt.Value * rInt.Value);
        }

        if (ReferenceEquals(left, expr.Left) && ReferenceEquals(right, expr.Right))
        {
            return expr;
        }

        return new Multiply(left, right);
    }

    private static Expr SimplifyPower(Power expr)
    {
        var @base = Simplify(expr.Base);
        var exponent = Simplify(expr.Exponent);

        if ((@base is NumberExpr zeroBase && zeroBase.IsZero) && 
            (exponent is NumberExpr zeroExp && zeroExp.IsZero)) 
        {
            return Undefined.Instance;
        }

        if (exponent is NumberExpr e)
        {
            if (e.IsZero) return new Integer(1);
            if (e.IsPositiveOne) return @base;
        }

        if (@base is NumberExpr b)
        {
            if (b.IsZero) return new Integer(0);
            if (b.IsPositiveOne) return new Integer(1);
        }

        if (@base is Integer bInt && exponent is Integer eInt) 
        {
            if (eInt.Value >= 0 && eInt.Value <= int.MaxValue)
            {
                return new Integer(BigInteger.Pow(bInt.Value, (int)eInt.Value));
            }
        }

        if (ReferenceEquals(@base, expr.Base) && ReferenceEquals(exponent, expr.Exponent))
        {
            return expr;
        }

        return new Power(@base, exponent);
    }

    // private static List<Expr> Flatten
}
