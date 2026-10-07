using System;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Rules.PowerRules;

public class NormativeStructure : PowerRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Power powerExpr) return expr;

        Expr baseExpr = powerExpr.Base;
        Expr expExpr = powerExpr.Exponent;

        if (Utility.IsZero(baseExpr))
        {
            if (Utility.IsZero(expExpr)) return Symbol.Indeterminate;
            if (Utility.IsNegative(expExpr)) return Symbol.ComplexInfinity;
            if (Utility.IsPositive(expExpr)) return new Integer(0);
        }

        if (Utility.IsZero(expExpr)) return new Integer(1);
        if (Utility.IsPositiveOne(expExpr)) return baseExpr;
        if (Utility.IsPositiveOne(baseExpr)) return new Integer(1);
        if (baseExpr is Power innerPower) return new Power(innerPower.Base, new Times(innerPower.Exponent, expExpr));

        return powerExpr;
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Power powerExpr) return false;

        Expr baseExpr = powerExpr.Base;
        Expr expExpr = powerExpr.Exponent;

        if (Utility.IsZero(baseExpr) || Utility.IsZero(expExpr) 
            || Utility.IsPositiveOne(baseExpr) || Utility.IsPositiveOne(expExpr)) return true;

        if (baseExpr is Power) return true;

        return false;
    }
}
