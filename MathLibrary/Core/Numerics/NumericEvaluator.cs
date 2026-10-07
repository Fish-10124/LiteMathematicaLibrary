using System;
using System.Numerics;
using MathLibrary.Core.Functions;

namespace MathLibrary.Core.Numerics;

public static class NumericEvaluator
{
    public static NumberExpr Reciprocal(NumberExpr expr)
    {
        switch (expr)
        {
            case Integer i:
                return Reciprocal(i);
            case Rational r:
                return Reciprocal(r);
            case MachineReal m:
                return Reciprocal(m);
            case BigReal b:
                return Reciprocal(b);
            case Complex c:
                return Reciprocal(c);
            default:
                throw new NotImplementedException();
        }
    }

    private static NumberExpr Reciprocal(Integer i)
    {
        if (i.IsPositiveOne || i.IsNegativeOne) return i;
        return new Rational(1, i.Value);
    }

    private static NumberExpr Reciprocal(Rational r)
    {
        var result = new Rational(r.Denominator, r.Numerator);
        return result.Denominator == 1 ? new Integer(result.Numerator) : result;
    }

    private static MachineReal Reciprocal(MachineReal m)
    {
        return new MachineReal(1.0 / m.Value);
    }

    private static BigReal Reciprocal(BigReal b)
    {
        if (b.IsZero) throw new DivideByZeroException("Cannot divide by zero.");

        var targetPrecision = b.Precision;

        var bDigitLength = Utility.GetDigitLength(BigInteger.Abs(b.Mantissa));

        var shift = targetPrecision + bDigitLength;
        var scaledNumerator = Utility.Pow(10, shift);

        var rawMantissa = scaledNumerator / b.Mantissa;
        var rawExponent = -b.Exponent - shift;

        var currentLength = Utility.GetDigitLength(BigInteger.Abs(rawMantissa));
        var finalMantissa = rawMantissa;
        var finalExponent = rawExponent;

        if (currentLength > targetPrecision)
        {
            var cutShift = currentLength - targetPrecision;
            finalMantissa /= Utility.Pow(10, cutShift);
            finalExponent += cutShift;
        }

        return new BigReal(finalMantissa, finalExponent, targetPrecision);
    }

    private static Complex Reciprocal(Complex c)
    {
        var aSquared = Multiply(c.Real, c.Real);
        var bSquared = Multiply(c.Imaginary, c.Imaginary);
        var normSqExpr = Add(aSquared, bSquared);

        if (normSqExpr is NumberExpr normSq)
        {
            if (normSq.IsZero) throw new DivideByZeroException("Cannot divide by zero Complex number.");

            var newReal = Multiply(aSquared, Reciprocal(normSq));

            var negImag = Multiply(new Integer(-1), c.Imaginary);
            var newImag = Multiply(negImag, Reciprocal(normSq));

            return new Complex(newReal, newImag);
        }

        throw new InvalidOperationException("Failed to evaluate complex norm squared.");
    }

    public static NumberExpr Power(NumberExpr baseNum, Integer expNum)
    {
        if (expNum.IsNegative) throw new ArgumentOutOfRangeException(nameof(expNum), "must be non-negative.");
        if (baseNum.IsZero) return new Integer(0);
        if (expNum.IsZero) return new Integer(1);

        switch (baseNum)
        {
            case Integer i:
                return Power(i, expNum);
            case Rational r:
                return Power(r, expNum);
            case MachineReal m:
                return Power(m, expNum);
            case BigReal b:
                return Power(b, expNum);
            case Complex c:
                return Power(c, expNum);
            default:
                throw new NotImplementedException();
        }
    }

    private static Integer Power(Integer baseNum, Integer expNum)
    {
        return new Integer(Utility.Pow(baseNum.Value, expNum.Value));
    }

    private static NumberExpr Power(Rational baseNum, Integer expNum)
    {
        var numerator = Utility.Pow(baseNum.Numerator, expNum.Value);
        var denominator = Utility.Pow(baseNum.Denominator, expNum.Value);
        var result = new Rational(numerator, denominator);
        return result.Denominator == 1 ? new Integer(result.Numerator) : result;
    }

    private static MachineReal Power(MachineReal baseNum, Integer expNum)
    {
        return new MachineReal(Math.Pow(baseNum.Value, (long)expNum.Value));
    }

    private static BigReal Power(BigReal baseNum, Integer expNum)
    {
        var newMantissa = Utility.Pow(baseNum.Mantissa, expNum.Value);
        var newExponent = baseNum.Exponent * expNum.Value;

        var currentLength = Utility.GetDigitLength(BigInteger.Abs(newMantissa));

        if (currentLength > baseNum.Precision)
        {
            var shift = currentLength - baseNum.Precision;
            newMantissa /= Utility.Pow(10, shift);
            newExponent += shift;
        }

        return new BigReal(newMantissa, newExponent, baseNum.Precision);
    }

    private static NumberExpr Power(Complex baseNum, Integer expNum)
    {
        var result = new Complex(new Integer(1), new Integer(0));
        var currentBase = baseNum;
        var exp = expNum.Value;

        while (exp > 0)
        {
            if (!exp.IsEven) result = (Complex)Multiply(result, currentBase);
            currentBase = (Complex)Multiply(currentBase, currentBase);
            exp >>= 1;
        }

        return result.Imaginary.IsZero ? result.Real : result;
    }

    public static NumberExpr Multiply(NumberExpr x, NumberExpr y)
    {
        var target = x.Kind > y.Kind ? x.Kind : y.Kind;

        x = Promote(x, target);
        y = Promote(y, target);

        switch ((x, y))
        {
            case (Integer i1, Integer i2):
                return Multiply(i1, i2);
            case (Rational r1, Rational r2):
                return Multiply(r1, r2);
            case (MachineReal m1, MachineReal m2):
                return Multiply(m1, m2);
            case (BigReal b1, BigReal b2):
                return Multiply(b1, b2);
            case (Complex c1, Complex c2):
                return Multiply(c1, c2);
            default:
                throw new NotImplementedException();
        }
    }

    private static Integer Multiply(Integer x, Integer y)
    {
        return new Integer(x.Value * y.Value);
    }

    private static NumberExpr Multiply(Rational x, Rational y)
    {
        var numerator = x.Numerator * y.Numerator;
        var denominator = x.Denominator * y.Denominator;
        var result = new Rational(numerator, denominator);
        return result.Denominator == 1 ? new Integer(result.Numerator) : result;
    }

    private static MachineReal Multiply(MachineReal x, MachineReal y)
    {
        return new MachineReal(x.Value * y.Value);
    }

    private static BigReal Multiply(BigReal x, BigReal y)
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
            finalMantissa /= Utility.Pow(10, shift);
            finalExponent += shift;
        }

        return new BigReal(finalMantissa, finalExponent, targetPrecision);
    }

    private static NumberExpr Multiply(Complex x, Complex y)
    {
        var ac = Multiply(x.Real, y.Real);
        var bd = Multiply(x.Imaginary, y.Imaginary);
        var ad = Multiply(x.Real, y.Imaginary);
        var bc = Multiply(x.Imaginary, y.Real);

        var newReal = Add(ac, Additive(bd));
        var newImag = Add(ad, bc);

        var result = new Complex(newReal, newImag);
        return result.Imaginary.IsZero ? result.Real : result;
    }

    public static NumberExpr Add(NumberExpr x, NumberExpr y)
    {
        var target = x.Kind > y.Kind ? x.Kind : y.Kind;

        x = Promote(x, target);
        y = Promote(y, target);

        switch ((x, y))
        {
            case (Integer i1, Integer i2):
                return Add(i1, i2);
            case (Rational r1, Rational r2):
                return Add(r1, r2);
            case (MachineReal m1, MachineReal m2):
                return Add(m1, m2);
            case (BigReal b1, BigReal b2):
                return Add(b1, b2);
            case (Complex c1, Complex c2):
                return Add(c1, c2);
            default:
                throw new NotImplementedException();
        }
    }

    private static Integer Add(Integer x, Integer y)
    {
        return new Integer(x.Value + y.Value);
    }

    private static NumberExpr Add(Rational x, Rational y)
    {
        var numerator = x.Numerator * y.Denominator + x.Denominator * y.Numerator;
        var denominator = x.Denominator * y.Denominator;
        var result = new Rational(numerator, denominator);
        return result.Denominator == 1 ? new Integer(result.Numerator) : result;
    }

    private static MachineReal Add(MachineReal x, MachineReal y)
    {
        return new MachineReal(x.Value + y.Value);
    }

    private static BigReal Add(BigReal x, BigReal y)
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
            m1 *= Utility.Pow(10, diff);
            baseExponent = e2;
        }
        else if (e2 > e1)
        {
            BigInteger diff = e2 - e1;
            m2 *= Utility.Pow(10, diff);
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
            finalMantissa /= Utility.Pow(10, shift);
            finalExponent += shift;
        }

        return new BigReal(finalMantissa, finalExponent, targetPrecision);
    }

    private static NumberExpr Add(Complex x, Complex y)
    {
        var newReal = Add(x.Real, y.Real);
        var newImag = Add(x.Imaginary, y.Imaginary);
        var result = new Complex(newReal, newImag);
        return result.Imaginary.IsZero ? result.Real : result;
    }
    
    public static NumberExpr Additive(NumberExpr expr)
    {
        switch (expr)
        {
            case Integer i:
                return Additive(i);
            case Rational r:
                return Additive(r);
            case MachineReal m:
                return Additive(m);
            case BigReal b:
                return Additive(b);
            case Complex c:
                return Additive(c);
            default:
                throw new NotImplementedException();
        }
    }

    private static Integer Additive(Integer i)
    {
        return new Integer(-i.Value);
    }

    private static Rational Additive(Rational r)
    {
        return new Rational(-r.Numerator, r.Denominator);
    }

    private static MachineReal Additive(MachineReal m)
    {
        return new MachineReal(-m.Value);
    }

    private static BigReal Additive(BigReal b)
    {
        return new BigReal(-b.Mantissa, b.Exponent, b.Precision);
    }

    private static Complex Additive(Complex c)
    {
        return new Complex(Additive(c.Real), Additive(c.Imaginary));
    }

    private static NumberExpr Promote(NumberExpr value, NumberKind target)
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
