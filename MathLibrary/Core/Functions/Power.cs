using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Symbols;

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

        if (evalExp is NumberExpr numExp0 && numExp0.IsZero)
        {
            return new Integer(1);
        }

        if (evalExp is NumberExpr numExp1 && numExp1.IsPositiveOne)
        {
            return evalBase;
        }

        if (evalBase is NumberExpr numBase1 && numBase1.IsPositiveOne)
        {
            return new Integer(1);
        }

        if (evalBase is NumberExpr numBase0 && numBase0.IsZero)
        {
            return new Integer(0);
        }

        if (evalBase is Power nestedPower)
        {
            Expr newExp = new Times(nestedPower.Exponent, evalExp).Evaluate();
            return new Power(nestedPower.Base, newExp).Evaluate();
        }

        if (evalBase is NumberExpr baseNum && evalExp is NumberExpr expNum)
        {
            var calculated = EvaluateNumericPower(baseNum, expNum);
            if (calculated != null)
            {
                return calculated;
            }
        }

        if (!ReferenceEquals(evalBase, Base) || !ReferenceEquals(evalExp, Exponent))
        {
            return new Power(evalBase, evalExp);
        }

        return this;
    }

    private static Expr? EvaluateNumericPower(NumberExpr baseNum, NumberExpr expNum)
    {
        if (baseNum.IsZero && expNum.IsZero) return Undefined.Instance;

        if (baseNum is Integer baseInt && expNum is Integer expInt)
        {
            if (expInt.Value >= 0 && expInt.Value <= int.MaxValue)
            {
                var resultValue = System.Numerics.BigInteger.Pow(baseInt.Value, (int)expInt.Value);
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

