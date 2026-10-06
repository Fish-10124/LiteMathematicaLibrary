using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Functions;

public sealed class Times : FunctionExpr
{
    public IReadOnlyList<Expr> Terms => this.Arguments;

    public Times(params Expr[] factors) : base(nameof(Times), factors) { }

    public override Expr Evaluate()
    {
        var flattenedTerms = FlattenAndEvaluate(Terms);

        NumberExpr numericProduct = new Integer(1);
        var symbolicPowers = new Dictionary<Expr, NumberExpr>();

        foreach (var term in flattenedTerms)
        {
            if (term is NumberExpr num)
            {
                // 遇到 0 直接短路返回 0
                if (num.IsZero) return num;

                numericProduct = numericProduct.Multiply(num);
            }
            else
            {
                var (baseExpr, exp) = ExtractExponent(term);
                if (symbolicPowers.TryGetValue(baseExpr, out var currentExp))
                {
                    symbolicPowers[baseExpr] = currentExp.Add(exp);
                }
                else
                {
                    symbolicPowers[baseExpr] = exp;
                }
            }
        }

        var resultTerms = new List<Expr>();

        if (!numericProduct.IsPositiveOne || symbolicPowers.Count == 0)
        {
            resultTerms.Add(numericProduct);
        }

        foreach (var (baseExpr, exp) in symbolicPowers)
        {
            // x^0 -> 1 (忽略)
            if (exp.IsZero) continue;

            // x^1 -> x
            if (exp.IsPositiveOne)
            {
                resultTerms.Add(baseExpr);
            }
            else
            {
                // x^n -> Power(x, n)
                resultTerms.Add(new Power(baseExpr, exp));
            }
        }

        if (resultTerms.Count == 0) return numericProduct;
        if (resultTerms.Count == 1) return resultTerms[0];

        return new Times(resultTerms.ToArray());
    }

    private static IReadOnlyList<Expr> FlattenAndEvaluate(IEnumerable<Expr> inputTerms)
    {
        var result = new List<Expr>();

        foreach (var term in inputTerms)
        {
            var evaluated = term.Evaluate();

            if (evaluated is Times nestedTimes)
            {
                result.AddRange(FlattenAndEvaluate(nestedTimes.Arguments));
            }
            else
            {
                result.Add(evaluated);
            }
        }

        return result;
    }

    private static (Expr BaseExpr, NumberExpr Exponent) ExtractExponent(Expr expr)
    {
        if (expr is Power power && power.Arguments.Count == 2 && power.Arguments[1] is NumberExpr numExp)
        {
            return (power.Arguments[0], numExp);
        }

        return (expr, new Integer(1));
    }

    public override string ToString()
    {
        return $"({string.Join("*", this.Terms)})";
    }
}
