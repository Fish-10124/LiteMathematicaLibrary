using System;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Rules.PowerRules;

public class NormativeStructure : PowerRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Power powerExpr) return expr;
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Power powerExpr) return false;

        return false;
    }
}
