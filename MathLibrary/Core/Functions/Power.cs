using MathLibrary.Core.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Functions;

public sealed class Power : FunctionExpr
{
    public Expr Base => Arguments[0];
    public Expr Exponent => Arguments[1];

    public Power(Expr @base, Expr exponent) : base(nameof(Power), @base, exponent) { }

    public override Expr Evaluate()
    {
        Expr evalBase = Base.Evaluate();
        Expr evalExp = Exponent.Evaluate();

        if (evalBase.Equals(Symbols.Undefined) || evalExp.Equals(Symbols.Undefined)) return Symbols.Undefined;
        if (evalBase.Equals(Symbols.Indeterminate) || evalExp.Equals(Symbols.Indeterminate)) return Symbols.Indeterminate;
        if (Utility.IsZero(evalBase) && Utility.IsZero(evalExp)) return Symbols.Indeterminate;
        if (Utility.IsZero(evalBase) && Utility.IsNegative(evalExp)) return Symbols.ComplexInfinity;
        if (evalBase.Equals(Symbols.ComplexInfinity) && Utility.IsZero(evalExp)) return Symbols.Indeterminate;
        if (evalBase.Equals(Symbols.ComplexInfinity) && Utility.IsNegative(evalExp)) return new Integer(0);
        if (Utility.IsZero(evalExp)) return new Integer(1);
        if (Utility.IsPositiveOne(evalExp)) return evalBase;
        if (Utility.IsPositiveOne(evalBase)) return new Integer(1);
        if (Utility.IsZero(evalBase)) return new Integer(0);

        if (evalBase is NumberExpr baseNum && evalExp is NumberExpr expNum)
        {
            var calculated = EvaluateNumericPower(baseNum, expNum);
            if (calculated != null)
            {
                return calculated;
            }
        }

        if (evalBase is Times timesBase)
        {
            var distributedTerms = new List<Expr>();
            foreach (var term in timesBase.Terms)
            {
                distributedTerms.Add(new Power(term, evalExp).Evaluate());
            }

            return new Times(distributedTerms.ToArray()).Evaluate();
        }

        if (evalExp is Integer expInt && evalBase is Integer baseInt && expInt.Value < 0)
        {
            var posExp = BigInteger.Abs(expInt.Value);
            var denominator = BigInteger.Pow(baseInt.Value, (int)posExp);
            return new Rational(1, denominator);
        }
        

        if (!ReferenceEquals(evalBase, Base) || !ReferenceEquals(evalExp, Exponent))
        {
            return new Power(evalBase, evalExp);
        }

        return this;
    }

    private static Integer? EvaluateNumericPower(NumberExpr baseNum, NumberExpr expNum)
    {
        if (baseNum is Integer baseInt && expNum is Integer expInt)
        {
            if (expInt.Value >= 0 && expInt.Value <= int.MaxValue)
            {
                var resultValue = BigInteger.Pow(baseInt.Value, (int)expInt.Value);
                return new Integer(resultValue);
            }
        }

        return null;
    }

    public override Expr Expand()
    {
        var expandedBase = Base.Expand();
        var expandedExp = Exponent.Expand();

        if (expandedBase is Plus && expandedExp is Integer expInt && expInt.Value > 1 && expInt.Value <= 10)
        {
            var count = (int)expInt.Value;
            var factors = new Expr[count];
            for (int i = 0; i < count; i++)
            {
                factors[i] = expandedBase;
            }

            return new Times(factors).Expand();
        }

        return new Power(expandedBase, expandedExp).Evaluate();
    }

    public override string ToString()
    {
        return $"({Base}^{Exponent})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Power other) return Equals(Base, other.Base) && Equals(Exponent, other.Exponent);
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Base, Exponent);
    }
}

