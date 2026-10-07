using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Rules.TimesRules;

public sealed class TermsCollecting : TimesRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Times times) return expr;

        var baseToExponent = new Dictionary<Expr, Expr>();
        var baseOrder = new List<Expr>();

        foreach (var factor in times.Factors)
        {
            var (baseExpr, expExpr) = ExtractExponent(factor);

            if (baseToExponent.TryGetValue(baseExpr, out var currentExp))
            {
                baseToExponent[baseExpr] = new Plus(currentExp, expExpr);
            }
            else
            {
                baseToExponent[baseExpr] = expExpr;
                baseOrder.Add(baseExpr);
            }
        }

        var newFactors = new List<Expr>();
        foreach (var baseExpr in baseOrder)
        {
            Expr combinedExp = baseToExponent[baseExpr];

            if (Utility.IsZero(combinedExp)) continue;
            if (Utility.IsPositiveOne(combinedExp)) newFactors.Add(baseExpr);
            else newFactors.Add(new Power(baseExpr, combinedExp));
        }

        if (newFactors.Count == 0) return new Integer(1);
        if (newFactors.Count == 1) return newFactors[0];

        return new Times(newFactors.ToArray());
    }

    private static (Expr BaseExpr, NumberExpr Exponent) ExtractExponent(Expr expr)
    {
        if (expr is Power power && power.Arguments.Count == 2 && power.Arguments[1] is NumberExpr numExp)
        {
            return (power.Arguments[0], numExp);
        }

        return (expr, new Integer(1));
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Times times) return false;

        var seenBases = new HashSet<Expr>();
        foreach (var factor in times.Factors)
        {
            var (baseExpr, _) = ExtractExponent(factor);

            if (!seenBases.Add(baseExpr)) return true;
        }
        return false;
    }
}
