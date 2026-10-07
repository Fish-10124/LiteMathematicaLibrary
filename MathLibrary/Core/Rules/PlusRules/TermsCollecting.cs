using System;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Rules.PlusRules;

public class TermsCollecting : PlusRules
{
    public override Expr Apply(Expr expr)
    {
        throw new NotImplementedException();
    }

    private static (NumberExpr Coeff, Expr Base) ExtractCoefficient(Expr expr)
    {
        if (expr is Times times)
        {
            var numTerms = times.Factors.OfType<NumberExpr>().ToList();
            var nonNumTerms = times.Factors.Where(t => t is not NumberExpr).ToList();

            NumberExpr coeff = new Integer(1);
            foreach (var num in numTerms)
            {
                coeff = NumericEvaluator.MultiplyNumbers(coeff, num);
            }

            if (nonNumTerms.Count == 0) return (coeff, new Integer(1));
            if (nonNumTerms.Count == 1) return (coeff, nonNumTerms[0]);

            return (coeff, new Times(nonNumTerms.ToArray()).Evaluate());
        }

        if (expr is NumberExpr numExpr) return (numExpr, new Integer(1));
        return (new Integer(1), expr);
    }

    public override bool Match(Expr expr)
    {
        if (expr is not Plus plus) return false;

        var seenPatterns = new HashSet<Expr>();
        foreach (var arg in plus.Terms)
        {
            var (_, pattern) = ExtractCoefficient(arg);
            if (!seenPatterns.Add(pattern)) return true; 
        }

        return false;
    }
}
