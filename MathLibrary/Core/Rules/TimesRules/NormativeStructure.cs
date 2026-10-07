using System;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Rules.TimesRules;

public sealed class NormativeStructure : TimesRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Times timesExpr) return expr;

        var newFactors = new List<Expr>();

        CollectTerms(timesExpr, newFactors, out bool hasZero);

        if (hasZero) return new Integer(0);
        if (newFactors.Count == 0) return new Integer(1);
        if (newFactors.Count == 1) return newFactors[0];

        return new Times(newFactors.ToArray());
    }

    private static void CollectTerms(Times timesExpr, List<Expr> result, out bool hasZero)
    {
        hasZero = false;
        
        foreach (var factor in timesExpr.Factors)
        {
            if (Utility.IsZero(factor))
            {
                hasZero = true;
                return;
            }
            if (Utility.IsPositiveOne(factor)) continue;
            if (factor is Times times)
            {
                CollectTerms(times, result, out hasZero);
                if (hasZero) return;
            }
            else result.Add(factor);
        }
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Times timesExpr) return false;

        if (timesExpr.Factors.Count <= 1) return true;

        foreach (var factor in timesExpr.Factors)
        {
            if (Utility.IsZero(factor) || Utility.IsPositiveOne(factor) || factor is Times) return true;
        }

        return false;
    }
}
