using MathLibrary.Core.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Functions;

public sealed class Plus : FunctionExpr
{
    public IReadOnlyList<Expr> Terms => this.Arguments;

    public Plus(params Expr[] terms) : base(nameof(Plus), terms) {}

    public override Expr Evaluate()
    {
        var flattenedTerms = FlattenAndEvaluate(Terms);

        if (flattenedTerms.Any(t => t.Equals(Symbols.Undefined))) return Symbols.Undefined;
        if (flattenedTerms.Any(t => t.Equals(Symbols.Indeterminate))) return Symbols.Indeterminate;

        var complexInfCount = flattenedTerms.Count(t => t.Equals(Symbols.ComplexInfinity));
        if (complexInfCount > 1) return Symbols.Indeterminate;
        if (complexInfCount == 1) return Symbols.ComplexInfinity;

        NumberExpr numericSum = new Integer(0);
        var symbolicTerms = new Dictionary<Expr, NumberExpr>();

        foreach (var term in flattenedTerms)
        {
            if (term is NumberExpr num)
            {
                numericSum = numericSum.Add(num);
            }
            else
            {
                var (coeff, baseExpr) = ExtractCoefficient(term);

                if (baseExpr is NumberExpr baseNum) numericSum = numericSum.Add(coeff.Multiply(baseNum));
                else if (symbolicTerms.TryGetValue(baseExpr, out var currentCoeff)) symbolicTerms[baseExpr] = currentCoeff.Add(coeff);
                else symbolicTerms[baseExpr] = coeff;
            }
        }

        var resultTerms = new List<Expr>();
        if (!numericSum.IsZero)
        {
            resultTerms.Add(numericSum);
        }
        foreach (var (baseExpr, coeff) in symbolicTerms)
        {
            if (coeff.IsZero) continue;

            if (coeff.IsPositiveOne) resultTerms.Add(baseExpr);
            else resultTerms.Add(new Times(coeff, baseExpr).Evaluate());
        }

        if (resultTerms.Count == 0) return numericSum;
        if (resultTerms.Count == 1) return resultTerms[0];

        return new Plus(resultTerms.ToArray());
    }

    private static List<Expr> FlattenAndEvaluate(IEnumerable<Expr> inputTerms)
    {
        var result = new List<Expr>();

        foreach (var term in inputTerms)
        {
            var evaluated = term.Evaluate();

            if (evaluated is Plus nestedPlus)
            {
                result.AddRange(FlattenAndEvaluate(nestedPlus.Terms));
            }
            else
            {
                result.Add(evaluated);
            }
        }

        return result;
    }

    private static (NumberExpr Coeff, Expr Base) ExtractCoefficient(Expr expr)
    {
        if (expr is Times times)
        {
            var numTerms = times.Terms.OfType<NumberExpr>().ToList();
            var nonNumTerms = times.Terms.Where(t => t is not NumberExpr).ToList();

            NumberExpr coeff = new Integer(1);
            foreach (var num in numTerms)
            {
                coeff = coeff.Multiply(num);
            }

            if (nonNumTerms.Count == 0) return (coeff, new Integer(1));
            if (nonNumTerms.Count == 1) return (coeff, nonNumTerms[0]);

            return (coeff, new Times(nonNumTerms.ToArray()).Evaluate());
        }

        if (expr is NumberExpr numExpr) return (numExpr, new Integer(1));
        return (new Integer(1), expr);
    }

    public override Expr Expand()
    {
        var expandedTerms = Terms.Select(t => t.Expand()).ToArray();

        return new Plus(expandedTerms).Evaluate();
    }

    public override string ToString()
    {
        return $"({string.Join("+", this.Terms)})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Plus other && Terms.Count == other.Terms.Count)
        {
            return !Terms.Except(other.Terms).Any() && !other.Terms.Except(Terms).Any();
        }
        return false;
    }

    public override int GetHashCode()
    {
        var hash = 17;
        foreach (var term in Terms)
        {
            hash ^= Terms.GetHashCode();
        }
        return hash;
    }
}
