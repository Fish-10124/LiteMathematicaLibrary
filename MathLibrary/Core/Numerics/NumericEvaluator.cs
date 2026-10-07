using System;
using System.Numerics;
using MathLibrary.Core.Functions;

namespace MathLibrary.Core.Numerics;

public static class NumericEvaluator
{
    public static NumberExpr MultiplyNumbers(NumberExpr x, NumberExpr y)
    {
        var target = x.Kind > y.Kind ? x.Kind : y.Kind;

        x = NumberExpr.Promote(x, target);
        y = NumberExpr.Promote(y, target);

        switch ((x, y))
        {
            case (Integer i1, Integer i2):
                return MultiplyNumbers(i1, i2);
            case (Rational r1, Rational r2):
                return MultiplyNumbers(r1, r2);
            case (MachineReal m1, MachineReal m2):
                return MultiplyNumbers(m1, m2);
            case (BigReal b1, BigReal b2):
                return MultiplyNumbers(b1, b2);
            case (Complex c1, Complex c2):
                return MultiplyNumbers(c1, c2);
            default:
                throw new NotImplementedException();
        }
    }

    private static Integer MultiplyNumbers(Integer x, Integer y)
    {
        return new Integer(x.Value * y.Value);
    }

    private static Rational MultiplyNumbers(Rational x, Rational y)
    {
        var numerator = x.Numerator * y.Numerator;
        var denominator = x.Denominator * y.Denominator;
        return new Rational(numerator, denominator);
    }

    private static MachineReal MultiplyNumbers(MachineReal x, MachineReal y)
    {
        return new MachineReal(x.Value * y.Value);
    }

    private static NumberExpr MultiplyNumbers(BigReal x, BigReal y)
    {
        if (x.Mantissa.IsZero || y.Mantissa.IsZero)
        {
            return new BigReal(0, 0, BigInteger.Max(x.Precision, y.Precision));
        }

        BigInteger targetPrecision = BigInteger.Max(x.Precision, y.Precision);

        BigInteger rawMantissa = x.Mantissa * y.Mantissa;
        BigInteger rawExponent = x.Exponent + y.Exponent;

        var currentLength = Utility.GetDigitLength(BigInteger.Abs(rawMantissa));
        BigInteger finalMantissa = rawMantissa;
        BigInteger finalExponent = rawExponent;

        if (currentLength > targetPrecision)
        {
            BigInteger shift = currentLength - targetPrecision;
            finalMantissa /= BigInteger.Pow(10, (int)shift);
            finalExponent += shift;
        }

        return new BigReal(finalMantissa, finalExponent, targetPrecision);
    }

    private static NumberExpr MultiplyNumbers(Complex x, Complex y)
    {
        Expr ac = new Times(x.Real, y.Real);
        Expr bd = new Times(x.Imaginary, y.Imaginary);
        Expr ad = new Times(x.Real, y.Imaginary);
        Expr bc = new Times(x.Imaginary, y.Real);

        Expr newReal = new Plus(ac, new Times(new Integer(-1), bd)).Evaluate();
        Expr newImag = new Plus(ad, bc).Evaluate();

        return new Complex(newReal, newImag);
    }

    public static NumberExpr AddNumbers(NumberExpr x, NumberExpr y)
    {
        var target = x.Kind > y.Kind ? x.Kind : y.Kind;

        x = NumberExpr.Promote(x, target);
        y = NumberExpr.Promote(y, target);

        switch ((x, y))
        {
            case (Integer i1, Integer i2):
                return AddNumbers(i1, i2);
            case (Rational r1, Rational r2):
                return AddNumbers(r1, r2);
            case (MachineReal m1, MachineReal m2):
                return AddNumbers(m1, m2);
            case (BigReal b1, BigReal b2):
                return AddNumbers(b1, b2);
            case (Complex c1, Complex c2):
                return AddNumbers(c1, c2);
            default:
                throw new NotImplementedException();
        }
    }

    private static Integer AddNumbers(Integer x, Integer y)
    {
        return new Integer(x.Value + y.Value);
    }

    private static Rational AddNumbers(Rational x, Rational y)
    {
        var numerator = x.Numerator * y.Denominator + x.Denominator * y.Numerator;
        var denominator = x.Denominator * y.Denominator;
        return new Rational(numerator, denominator);
    }

    private static MachineReal AddNumbers(MachineReal x, MachineReal y)
    {
        return new MachineReal(x.Value + y.Value);
    }

    private static NumberExpr AddNumbers(BigReal x, BigReal y)
    {
        if (x.Mantissa.IsZero) return y;
        if (y.Mantissa.IsZero) return x;

        BigInteger targetPrecision = BigInteger.Max(x.Precision, y.Precision);

        BigInteger m1 = x.Mantissa;
        BigInteger m2 = y.Mantissa;
        BigInteger e1 = x.Exponent;
        BigInteger e2 = y.Exponent;
        BigInteger baseExponent;

        if (e1 > e2)
        {
            BigInteger diff = e1 - e2;
            m1 *= BigInteger.Pow(10, (int)diff);
            baseExponent = e2;
        }
        else if (e2 > e1)
        {
            BigInteger diff = e2 - e1;
            m2 *= BigInteger.Pow(10, (int)diff);
            baseExponent = e1;
        }
        else
        {
            baseExponent = e1;
        }

        BigInteger sumMantissa = m1 + m2;
        if (sumMantissa.IsZero) return new BigReal(0, 0, targetPrecision);

        var currentLength = Utility.GetDigitLength(BigInteger.Abs(sumMantissa));
        BigInteger finalMantissa = sumMantissa;
        BigInteger finalExponent = baseExponent;

        if (currentLength > targetPrecision)
        {
            BigInteger shift = currentLength - targetPrecision;
            finalMantissa /= BigInteger.Pow(10, (int)shift);
            finalExponent += shift;
        }

        return new BigReal(finalMantissa, finalExponent, targetPrecision);
    }

    private static Complex AddNumbers(Complex x, Complex y)
    {
        Expr newReal = new Plus(x.Real, y.Real).Evaluate();
        Expr newImag = new Plus(x.Imaginary, y.Imaginary).Evaluate();
        return new Complex(newReal, newImag);
    }
}
