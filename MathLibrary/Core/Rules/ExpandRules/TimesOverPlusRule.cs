using System;
using MathLibrary.Core.Functions;

namespace MathLibrary.Core.Rules.ExpandRules;

public sealed class TimesOverPlusRule : ExpandRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Times times) return expr;

        var index = Array.FindIndex(times.Factors.ToArray(), f => f is Plus);
        if (index == -1) return expr;
        
        var others = times.Factors.Where((_, i) => i != index).ToArray();
        var plus = (Plus)times.Factors[index];
        return new Plus(plus.Terms.Select(t => new Times(others.Append(t).ToArray())).ToArray());
    }

    public override bool Match(Expr expr)
    {
        return expr is Times times && times.Factors.Any(f => f is Plus);
    }
}
