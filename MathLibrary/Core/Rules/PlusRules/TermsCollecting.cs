using System;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Rules.PlusRules;

public class TermsCollecting : PlusRules
{
    public override Expr Apply(Expr expr)
    {
        if (expr is not Plus plus) return expr;

        var patternToCoeff = new Dictionary<Expr, NumberExpr>();
        var patternOrder = new List<Expr>();

        foreach (var term in plus.Terms)
        {
            var (coeff, kernel) = ExtractCoefficient(term);

            if (patternToCoeff.TryGetValue(kernel, out var currentCoeff))
            {
                patternToCoeff[kernel] = NumericEvaluator.Add(currentCoeff, coeff);
            }
            else
            {
                patternToCoeff[kernel] = coeff;
                patternOrder.Add(kernel);
            }
        }

        var newTerms = new List<Expr>();

        foreach (var kernel in patternOrder)
        {
            NumberExpr finalCoeff = patternToCoeff[kernel];

            if (Utility.IsZero(finalCoeff)) continue;

            if (Utility.IsPositiveOne(kernel)) newTerms.Add(finalCoeff);
            else if (Utility.IsPositiveOne(finalCoeff)) newTerms.Add(kernel);
            else newTerms.Add(new Times(finalCoeff, kernel));
        }

        if (newTerms.Count == 0) return new Integer(0);
        if (newTerms.Count == 1) return newTerms[0];

        return new Plus(newTerms.ToArray());
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
                coeff = NumericEvaluator.Multiply(coeff, num);
            }

            if (nonNumTerms.Count == 0) return (coeff, new Integer(1));
            if (nonNumTerms.Count == 1) return (coeff, nonNumTerms[0]);

            return (coeff, new Times(nonNumTerms.ToArray()));
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
