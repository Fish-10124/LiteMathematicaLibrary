using System;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Operations;

namespace MathLibrary.Core.Utils;

public class ExprComparer : IComparer<Expr>
{
    public static ExprComparer Instance { get; } = new ExprComparer();

    public int Compare(Expr? x, Expr? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        if (x is NumberExpr nx && y is NumberExpr ny)
        {
            return CompareNumbers(nx, ny);
        }
        if (x is NumberExpr) return -1;
        if (y is NumberExpr) return 1;

        var typeCompare = string.Compare(x.GetType().Name, y.GetType().Name, StringComparison.Ordinal);
        if (typeCompare != 0) return typeCompare;

        switch ((x, y))
        {
            case (Add a1, Add a2):
                return CombineCompare(Compare(a1.Left, a2.Left), Compare(a1.Right, a2.Right));
            case (Multiply m1, Multiply m2):
                return CombineCompare(Compare(m1.Left, m2.Left), Compare(m1.Right, m2.Right));
            case (Power p1, Power p2):
                return CombineCompare(Compare(p1.Base, p2.Base), Compare(p1.Exponent, p2.Exponent));
            default:
                return string.Compare(x.ToString(), y.ToString(), StringComparison.Ordinal);
        }
    }

    private static int CompareNumbers(NumberExpr x, NumberExpr y)
    {
        if (x is Integer i1 && y is Integer i2)
        {
            return i1.Value.CompareTo(i2.Value);
        }
        return string.Compare(x.ToString(), y.ToString(), StringComparison.Ordinal);
    }

    private static int CombineCompare(int first, int second)
    {
        return first != 0 ? first : second;
    }
}
