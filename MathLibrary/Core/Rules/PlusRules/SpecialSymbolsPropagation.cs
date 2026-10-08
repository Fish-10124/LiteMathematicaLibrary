using System;
using MathLibrary.Core.Functions;

namespace MathLibrary.Core.Rules.PlusRules;

public class SpecialSymbolsPropagation : PlusRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Plus plusExpr) return expr;

        if (plusExpr.Terms.Any(t => t == Symbol.Indeterminate)) return Symbol.Indeterminate;
        
        var infinityCount = plusExpr.Terms.Count(t => t == Symbol.ComplexInfinity);
        if (infinityCount >= 2) return Symbol.Indeterminate;

        if (infinityCount == 1) return Symbol.ComplexInfinity;

        return expr;
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Plus plus) return false;

        return plus.Terms.Any(t => t == Symbol.ComplexInfinity || t == Symbol.Indeterminate);
    }
}
