using System;
using MathLibrary.Core.Operations;

namespace MathLibrary.Core.Rules.ExpandRules;

public sealed class NestedExpandRule : ExpandRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not FunctionExpr func) return expr;

        var newArgs = func.Arguments.Select(Expander.Expand).ToArray();
        if (func.Arguments.SequenceEqual(newArgs)) return expr;
        return func.Create(newArgs);
    }

    public override bool Match(Expr expr)
    {
        return expr is FunctionExpr;
    }
}
