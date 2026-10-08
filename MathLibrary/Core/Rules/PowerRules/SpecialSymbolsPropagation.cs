using System;
using MathLibrary.Core.Functions;

namespace MathLibrary.Core.Rules.PowerRules;

public class SpecialSymbolsPropagation : PowerRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Power powerExpr) return expr;

        var @base = powerExpr.Base;
        var exponent = powerExpr.Exponent;

        if (@base == Symbol.Indeterminate || exponent == Symbol.Indeterminate) return Symbol.Indeterminate;

        if (@base == Symbol.ComplexInfinity)
        {
            if (Utility.IsZero(exponent)) return Symbol.Indeterminate;
            if (Utility.IsPositive(exponent)) return Symbol.ComplexInfinity;
        }

        return expr;
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Power power) return false;

        return power.Base == Symbol.ComplexInfinity
            || power.Base == Symbol.Indeterminate
            || power.Exponent == Symbol.Indeterminate;
    }
}
