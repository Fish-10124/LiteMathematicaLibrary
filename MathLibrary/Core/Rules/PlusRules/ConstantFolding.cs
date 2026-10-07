using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Rules.PlusRules;

public class ConstantFolding : PlusRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Plus plusExpr) return expr;

        var nonNumbers = new List<Expr>();
        NumberExpr accumulated = new Integer(0);

        foreach (var term in plusExpr.Terms)
        {
            if (term is NumberExpr num) accumulated = NumericEvaluator.AddNumbers(accumulated, num);
            else nonNumbers.Add(term);
        }

        if (Utility.IsZero(accumulated) && nonNumbers.Count > 0)
        {
            if (nonNumbers.Count == 1) return nonNumbers[0];
            return new Plus(nonNumbers.ToArray());
        }

        if (nonNumbers.Count == 0) return accumulated;

        nonNumbers.Add(accumulated);
        return new Plus(nonNumbers.ToArray());
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Plus plusExpr) return false;
        return plusExpr.Terms.Count(t => t is NumberExpr) > 1;
    }
}
