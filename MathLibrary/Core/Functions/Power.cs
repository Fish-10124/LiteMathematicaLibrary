using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathLibrary.Core.Numerics;

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

        if (evalBase == Symbols.Undefined || evalExp == Symbols.Undefined) return Symbols.Undefined;
        if (evalBase == Symbols.Indeterminate || evalExp == Symbols.Indeterminate) return Symbols.Indeterminate;
        if (Utility.IsZero(evalBase) && Utility.IsZero(evalExp)) return Symbols.Indeterminate;
        if (Utility.IsZero(evalBase) && Utility.IsNegative(evalExp)) return Symbols.ComplexInfinity;
        if (evalBase == Symbols.ComplexInfinity && Utility.IsZero(evalExp)) return Symbols.Indeterminate;
        if (evalBase == Symbols.ComplexInfinity && Utility.IsNegative(evalExp)) return new Integer(0);
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

        if (evalBase is Integer baseInt && evalExp is Integer expInt && expInt.Value < 0)
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

    private static Integer EvaluateNumericPower(NumberExpr baseNum, NumberExpr expNum)
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

    public override string ToString()
    {
        return $"({Base}^{Exponent})";
    }
}

