using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Rules.TimesRules;

public class ConstantFolding : TimesRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Times timesExpr) return expr;

        var nonNumbers = new List<Expr>();
        NumberExpr accumulated = new Integer(1);

        foreach (var factor in timesExpr.Factors)
        {
            if (factor is NumberExpr num)
            {
                accumulated = NumericEvaluator.MultiplyNumbers(accumulated, num);
                if (Utility.IsZero(accumulated)) return accumulated;
            }
            else nonNumbers.Add(factor);
        }

        if (Utility.IsPositiveOne(accumulated) && nonNumbers.Count > 0)
        {
            if (nonNumbers.Count == 1) return nonNumbers[0];
            return new Times(nonNumbers.ToArray());
        }

        if (nonNumbers.Count == 0) return accumulated;

        nonNumbers.Insert(0, accumulated);
        return new Times(nonNumbers.ToArray());
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Plus plusExpr) return false;
        return plusExpr.Terms.Count(t => t is NumberExpr) > 1;
    }
}
