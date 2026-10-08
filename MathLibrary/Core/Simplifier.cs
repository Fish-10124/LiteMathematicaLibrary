using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Rules;

namespace MathLibrary.Core;

public static class Simplifier
{

    public static Expr Simplify(Expr expr)
    {
        if (expr is not FunctionExpr func) return expr;

        expr = func.Create(func.Arguments.Select(Simplify).ToArray());

        return ApplyRules(expr);
    }

    private static Expr ApplyRules(Expr expr)
    {
        Expr previous;

        do
        {
            previous = expr;

            foreach (var rule in RuleRegistry.GetRules(expr))
            {
                if (rule.Match(expr)) expr = rule.Apply(expr);
            }

            if (expr is FunctionExpr func) expr = func.Create(func.Arguments.Select(Simplify).ToArray());
        }
        while (!expr.Equals(previous));

        return expr;
    }
}
