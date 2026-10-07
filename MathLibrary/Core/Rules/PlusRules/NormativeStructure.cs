using System;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Rules.PlusRules;

public class NormativeStructure : PlusRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Plus plusExpr) return expr;

        var newTerms = new List<Expr>();

        CollectTerms(plusExpr, newTerms);

        if (newTerms.Count == 0) return new Integer(0);
        if (newTerms.Count == 1) return newTerms[0];

        return new Plus(newTerms.ToArray());
    }

    private static void CollectTerms(Plus plusExpr, List<Expr> result)
    {
        foreach (var term in plusExpr.Terms)
        {
            if (Utility.IsZero(term)) continue;

            if (term is Plus plus) CollectTerms(plus, result);
            else result.Add(term);
        }
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Plus plusExpr) return false;

        if (plusExpr.Terms.Count <= 1) return true;

        foreach (var term in plusExpr.Terms)
        {
            if (Utility.IsZero(term) || term is Plus) return true;
        }

        return false;
    }
}
