using System;
using System.Numerics;

namespace MathLibrary.Core.Numerics;

public abstract class NumberExpr : Expr
{
    public virtual bool IsZero => false;

    public virtual bool IsPositiveOne => false;

    public virtual bool IsNegativeOne => false;

    public virtual bool IsPositive => false;

    public virtual bool IsNegative => false;

    public abstract NumberKind Kind { get; }

    public static NumberExpr Promote(NumberExpr value, NumberKind target)
    {
        if (value.Kind == target) return value;

        switch ((value, target))
        {
            case (Integer i, NumberKind.Rational):
                return new Rational(i.Value, 1);
            case (Integer i, NumberKind.Real):
                return new BigReal(i.Value, 0, Utility.GetDigitLength(BigInteger.Abs(i.Value)));
            case (Rational r, NumberKind.Real):
                return RationalToBigReal(r);
            case (_, NumberKind.Complex):
                return new Complex(value, new Integer(0));
            default:
                throw new NotImplementedException();
        }
    }

    private static BigReal RationalToBigReal(Rational r, BigInteger? defaultPrecision = null)
    {
        var numerator = r.Numerator;
        var denominator = r.Denominator;

        if (numerator.IsZero)
        {
            return new BigReal(0, 0);
        }

        BigInteger targetPrecision = defaultPrecision ?? 50;

        var numLen = Utility.GetDigitLength(BigInteger.Abs(numerator));
        var denLen = Utility.GetDigitLength(denominator);

        BigInteger k = targetPrecision + denLen - numLen;
        if (k < 0) k = 0;

        BigInteger scaledNumerator = numerator * Utility.Pow(10, k);
        BigInteger mantissa = scaledNumerator / denominator;
        BigInteger exponent = -k;

        while (!mantissa.IsZero && mantissa % 10 == 0)
        {
            mantissa /= 10;
            exponent++;
        }

        return new BigReal(mantissa, exponent);
    }
}
