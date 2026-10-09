using System;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Rules.ExpandRules;

public sealed class PowerOfSumRule : ExpandRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Power power) return expr;
        if (power.Exponent is not Integer exponent) return expr;

        var expNum = exponent.Value;

        if (expNum < int.MaxValue) return new Times(Enumerable.Repeat(power.Base, (int)expNum).ToArray());
        return expr;
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Power power) return false;

        return power.Base is Plus && power.Exponent is Integer i && i.IsPositive;
    }
}
