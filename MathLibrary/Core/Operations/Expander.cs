using System;
using MathLibrary.Core.Rules;

namespace MathLibrary.Core.Operations;

public static class Expander
{
    public static Expr Expand(Expr expr)
    {
        if (expr is not FunctionExpr) return expr;

        var current = expr;
        bool changed;

        do
        {
            changed = false;

            foreach (var rule in RuleRegistry.GetExpandRules())
            {
                if (!rule.Match(current)) continue;

                var result = rule.Apply(current);
                if (!result.Equals(current))
                {
                    current = result;
                    changed = true;
                    break;
                }
            }
        } while (changed);

        return current;
    }
}
