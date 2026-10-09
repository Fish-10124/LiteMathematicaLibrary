using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Operations;

public static class Collector
{
    public static Expr Coefficient(Expr expr, Symbol variable, BigInteger degree)
    {
        var table = BuildCollectTable(expr, variable);

        return table.TryGetValue(degree, out var coeff) ? coeff : new Integer(0);
    }

    public static Expr Collect(Expr expr, Symbol variable)
    {
        var table = BuildCollectTable(expr, variable);
        var terms = new List<Expr>();

        foreach(var pair in table)
        {
            var degree = pair.Key;
            var coeff = pair.Value;

            Expr term;
            if (degree == 0) term = coeff;
            else if (degree == 1) term = new Times(coeff, variable);
            else term = new Times(coeff, new Power(variable, new Integer(degree)));

            terms.Add(term);
        }

        return terms.Count == 1 ? terms[0] : new Plus(terms.ToArray());
    }

    private static BigInteger GetDegree(Expr expr, Symbol variable)
    {
        if (expr.Equals(variable)) return 1;

        if (expr is Power power && power.Base.Equals(variable) && power.Exponent is Integer n) return n.Value;
        if (expr is Times times)
        {
            BigInteger total = 0;
            foreach (var factor in times.Factors)
            {
                total += GetDegree(factor, variable);
            }
            return total;
        }
        return 0;
    }

    private static Expr RemoveVariable(Expr expr, Symbol variable)
    {
        if (expr.Equals(variable)) return new Integer(1);
        if (expr is Power power && power.Base.Equals(variable)) return new Integer(1);

        if (expr is Times times)
        {
            var factors = new List<Expr>();

            foreach (var f in times.Factors)
            {
                if (GetDegree(f, variable) == 0) factors.Add(f);
            }

            if (factors.Count == 0) return new Integer(1);
            if (factors.Count == 1) return factors[0];

            return new Times(factors.ToArray());
        }

        return expr;
    }

    private static Dictionary<BigInteger, Expr> BuildCollectTable(Expr expr, Symbol variable)
    {
        expr = Simplifier.FullSimplify(expr);

        var terms = expr is Plus plus ? plus.Terms : new[] { expr };
        var table = new Dictionary<BigInteger, List<Expr>>();

        foreach (var term in terms)
        {
            var degree = GetDegree(term, variable);
            var coeff = RemoveVariable(term, variable);

            table.TryAdd(degree, new List<Expr>());

            table[degree].Add(coeff);
        }

        return table.ToDictionary(
            p => p.Key, 
            p => Simplifier.Simplify(p.Value.Count == 1 ? p.Value[0] : new Plus(p.Value.ToArray()))
        );
    }
}
