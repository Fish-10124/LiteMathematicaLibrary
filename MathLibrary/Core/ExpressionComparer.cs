using System;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core;

public sealed class ExpressionComparer : IComparer<Expr>
{
    public static readonly ExpressionComparer Instance = new();

    private ExpressionComparer() { }

    public int Compare(Expr? x, Expr? y)
    {
        if (ReferenceEquals(x, y)) return 0;

        if (x == null) return -1;
        if (y == null) return 1;

        var rankX = GetRank(x);
        var rankY = GetRank(y);

        if (rankX != rankY) return rankX.CompareTo(rankY);

        return CompareSameType(x, y);
    }

    private static byte GetRank(Expr expr)
    {
        switch (expr)
        {
            case NumberExpr:
                return 0;
            case Symbol:
                return 1;
            case Power:
                return 2;
            case Function:
                return 3;
            case Times:
                return 4;
            case Plus:
                return 5;
            default:
                return byte.MaxValue;
        }
    }

    private int CompareSameType(Expr x, Expr y)
    {
        switch ((x, y))
        {
            case (Integer i1, Integer i2):
                return i1.Value.CompareTo(i2.Value);
            case (Rational r1, Rational r2):{
                var left = r1.Numerator * r2.Denominator;
                var right = r2.Numerator * r1.Denominator;
                return left.CompareTo(right);}
            case (Symbol s1, Symbol s2):
                return string.CompareOrdinal(s1.Name, s2.Name);
            case (Power p1, Power p2):{
                int cmp = Compare(p1.Base, p2.Base);
                return cmp != 0 ? cmp : Compare(p1.Exponent, p2.Exponent);}
            case (Function f1, Function f2):{
                int cmp = string.CompareOrdinal(f1.Name, f2.Name);
                return cmp != 0 ? cmp : CompareList(f1.Arguments, f2.Arguments);}
            case (Times t1, Times t2):
                return CompareList(t1.Factors, t2.Factors);
            case (Plus p1, Plus p2):
                return CompareList(p1.Terms, p2.Terms);
            default:
                return string.CompareOrdinal(x.ToString(), y.ToString());
        }
    }

    private int CompareList(IReadOnlyList<Expr> x, IReadOnlyList<Expr> y)
    {
        int countCmp = x.Count.CompareTo(y.Count);

        if (countCmp != 0) return countCmp;

        for (int i = 0; i < x.Count; i++)
        {
            int cmp = Compare(x[i], y[i]);
            if (cmp != 0) return cmp;
        }

        return 0;
    }
}
