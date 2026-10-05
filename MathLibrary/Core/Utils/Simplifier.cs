using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Operations;
using MathLibrary.Core.Symbols;

namespace MathLibrary.Core.Utils;

public static class Simplifier
{
    public static Expr Simplify(Expr expr)
    {
        switch (expr)
        {
            case Add add:
                return SimplifyAdd(add);
            case Multiply multiply:
                return SimplifyMultiply(multiply);
            case Power power:
                return SimplifyPower(power);

            default:
                return expr;
        }
    }

    private static Expr SimplifyAdd(Add expr)
    {
        var left = Simplify(expr.Left);
        var right = Simplify(expr.Right);

        var terms = FlattenAdd(new Add(left, right));

        for (int i = 0; i < terms.Count; i++)
        {
            terms[i] = Simplify(terms[i]);
        }

        var constantSum = BigInteger.Zero;
        var groupedTerms = new Dictionary<Expr, BigInteger>(ExprEqualityComparer.Instance);

        foreach (var term in terms)
        {
            if (term is NumberExpr num && num.IsZero) continue;

            var (coeff, baseExpr) = ExtractCoefficient(term);

            if (baseExpr is Integer bInt && bInt.Value == 1)
            {
                constantSum += coeff.Value;
            }
            else
            {
                if (groupedTerms.TryGetValue(baseExpr, out var currentCoeff))
                {
                    groupedTerms[baseExpr] = currentCoeff + coeff.Value;
                }
                else
                {
                    groupedTerms[baseExpr] = coeff.Value;
                }
            }
        }

        var resultList = new List<Expr>();

        if (constantSum != 0)
        {
            resultList.Add(new Integer(constantSum));
        }

        foreach (var (baseExpr, coeff) in groupedTerms)
        {
            if (coeff == 0) continue;

            if (coeff == 1)
            {
                resultList.Add(baseExpr);
            }
            else
            {
                resultList.Add(new Multiply(new Integer(coeff), baseExpr));
            }
        }

        if (resultList.Count == 0) return new Integer(0);
        if (resultList.Count == 1) return resultList[0];

        resultList.Sort(ExprComparer.Instance);

        Expr current = resultList[0];
        for (int i = 1; i < resultList.Count; i++)
        {
            current = new Add(current, resultList[i]);
        }

        return current;
    }

    private static Expr SimplifyMultiply(Multiply expr)
    {
        var left = Simplify(expr.Left);
        var right = Simplify(expr.Right);

        var factors = FlattenMultiply(new Multiply(left, right));

        for (int i = 0; i < factors.Count; i++)
        {
            factors[i] = Simplify(factors[i]);
        }

        if (factors.Any(f => f is NumberExpr num && num.IsZero))
        {
            return new Integer(0);
        }

        var constantProduct = new Rational(1, 1);
        var groupedFactors = new Dictionary<Expr, Expr>(ExprEqualityComparer.Instance);

        foreach (var factor in factors)
        {
            if (factor is NumberExpr num && num.IsPositiveOne) continue;

            if (factor is Integer i)
            {
                constantProduct = MultiRational(constantProduct, new Rational(i.Value, 1));
                continue;
            }
            if (factor is Rational r)
            {
                constantProduct = MultiRational(constantProduct, r);
                continue;
            }

            var (baseExpr, expExpr) = ExtractBaseAndExponent(factor);

            if (groupedFactors.TryGetValue(baseExpr, out var currentExp))
            {
                groupedFactors[baseExpr] = Simplify(new Add(currentExp, expExpr));
            }
            else
            {
                groupedFactors[baseExpr] = expExpr;
            }
        }

        var resultList = new List<Expr>();

        if (!constantProduct.IsPositiveOne || groupedFactors.Count == 0)
        {
            Expr constExpr = constantProduct.Denominator == 1
                ? new Integer(constantProduct.Numerator)
                : constantProduct;

            resultList.Add(constExpr);
        }

        foreach (var (baseExpr, expExpr) in groupedFactors)
        {
            if (expExpr is NumberExpr numExp && numExp.IsZero) continue;

            if (expExpr is NumberExpr expOne && expOne.IsPositiveOne)
            {
                resultList.Add(baseExpr);
            }
            else
            {
                resultList.Add(new Power(baseExpr, expExpr));
            }
        }

        if (resultList.Count == 0) return new Integer(1);
        if (resultList.Count == 1) return resultList[0];

        resultList.Sort(ExprComparer.Instance);

        Expr current = resultList[0];
        for (int i = 1; i < resultList.Count; i++)
        {
            current = new Multiply(current, resultList[i]);
        }

        return current;
    }

    private static Rational MultiRational(Rational x, Rational y)
    {
        return new Rational(x.Numerator * y.Numerator, x.Denominator * y.Denominator);
    }

    private static Expr SimplifyPower(Power expr)
    {
        var @base = Simplify(expr.Base);
        var exponent = Simplify(expr.Exponent);

        if ((@base is NumberExpr zeroBase && zeroBase.IsZero) && 
            (exponent is NumberExpr zeroExp && zeroExp.IsZero)) 
        {
            return Undefined.Instance;
        }

        if (exponent is NumberExpr e)
        {
            if (e.IsZero) return new Integer(1);
            if (e.IsPositiveOne) return @base;
        }

        if (@base is NumberExpr b)
        {
            if (b.IsZero) return new Integer(0);
            if (b.IsPositiveOne) return new Integer(1);
        }

        if (@base is Integer bInt && exponent is Integer eInt) 
        {
            if (eInt.Value < 0)
            {
                var posExp = (int)-eInt.Value;
                var denominator = BigInteger.Pow(bInt.Value, posExp);
                return Simplify(new Rational(1, denominator));
            }
            else if (eInt.Value <= int.MaxValue)
            {
                return new Integer(BigInteger.Pow(bInt.Value, (int)eInt.Value));
            }
        }

        if (@base is Multiply mul)
        {
            var leftPower = Simplify(new Power(mul.Left, exponent));
            var rightPower = Simplify(new Power(mul.Right, exponent));
            return Simplify(new Multiply(leftPower, rightPower));
        }

        if (ReferenceEquals(@base, expr.Base) && ReferenceEquals(exponent, expr.Exponent))
        {
            return expr;
        }

        return new Power(@base, exponent);
    }

    private static List<Expr> FlattenAdd(Expr expr)
    {
        var list = new List<Expr>();
        Collect(expr);
        return list;

        void Collect(Expr e)
        {
            if (e is Add add)
            {
                Collect(add.Left);
                Collect(add.Right);
            }
            else
            {
                list.Add(e);
            }
        }
    }

    private static (Integer Coeff, Expr Base) ExtractCoefficient(Expr expr)
    {
        if (expr is Integer i)
        {
            return (i, new Integer(1));
        }

        if (expr is Multiply m)
        {
            if (m.Left is Integer lInt) return (lInt, m.Right);
            if (m.Right is Integer rInt) return (rInt, m.Left);
        }

        return (new Integer(1), expr);
    }

    private static List<Expr> FlattenMultiply(Expr expr)
    {
        var list = new List<Expr>();
        Collect(expr);
        return list;

        void Collect(Expr e)
        {
            if (e is Multiply m)
            {
                Collect(m.Left);
                Collect(m.Right);
            }
            else
            {
                list.Add(e);
            }
        }
    }

    private static (Expr Base, Expr Exponent) ExtractBaseAndExponent(Expr expr)
    {
        if (expr is Power p)
        {
            return (p.Base, p.Exponent);
        }
        return (expr, new Integer(1));
    }
}
