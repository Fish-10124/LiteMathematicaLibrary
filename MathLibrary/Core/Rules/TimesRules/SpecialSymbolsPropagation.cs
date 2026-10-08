using System;
using MathLibrary.Core.Functions;

namespace MathLibrary.Core.Rules.TimesRules;

public class SpecialSymbolsPropagation : TimesRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Times timesExpr) return expr;

        if (timesExpr.Factors.Any(t => t == Symbol.Indeterminate)) return Symbol.Indeterminate;

        bool hasInfinity = timesExpr.Factors.Any(t => t == Symbol.ComplexInfinity);
        if (!hasInfinity) return expr;

        bool hasZero = timesExpr.Factors.Any(Utility.IsZero);
        if (hasZero) return Symbol.Indeterminate;
        return Symbol.ComplexInfinity;
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Times times) return false;

        return times.Factors.Any(t => t == Symbol.ComplexInfinity || t == Symbol.Indeterminate);
    }
}
