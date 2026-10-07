using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;
using System;

namespace MathLibrary.Core.Rules.PowerRules;

public sealed class ConstantFolding : PowerRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Power powerExpr) return expr;

        if (powerExpr.Base is not NumberExpr baseNum || powerExpr.Exponent is not Integer expNum) return expr;

        if (expNum.IsNegative)
        {
            var posExp = -expNum.Value;
            var denominator = NumericEvaluator.Power(baseNum, posExp);
            return NumericEvaluator.Reciprocal(denominator);
        }

        return NumericEvaluator.Power(baseNum, expNum);
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Power powerExpr) return false;

        return powerExpr.Base is NumberExpr && powerExpr.Exponent is Integer;
    }
}
