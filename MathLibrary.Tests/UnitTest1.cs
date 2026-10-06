using MathLibrary.Core;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Functions;
using Xunit.Abstractions;
using BigInteger = System.Numerics.BigInteger;

namespace MathLibrary.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _output;

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void DividedByZero_Returns_Undefined()
    {
        var expr = 5 * (new Symbol("x") ^ -1);
        var result = expr.Evaluate();
        Assert.Equal("(5*(x^-1))", result.ToString());
        // Now divide by zero
        var divByZero = result / 0;
        var finalResult = divByZero.Evaluate();
        Assert.Equal("ComplexInfinity", finalResult.ToString());
        // Now multiply by zero
        var multiplyByZero = finalResult * 0;
        var zeroResult = multiplyByZero.Evaluate();
        Assert.Equal("Undefined", zeroResult.ToString());
    }

    [Fact]
    public void NumberExpr_Ranks_And_Operators()
    {
        var i1 = new Integer(1);
        var i2 = new Integer(2);
        var sum = (i1 + i2).Evaluate(); // Integer + Integer
        Assert.IsType<Integer>(sum);
        Assert.Equal("3", sum.ToString());

        var r = new Rational(4, 6); // reduced to 2/3
        Assert.Equal("2/3", r.ToString());

        // Mixed rank: Integer * Rational -> Rational or Integer
        var mixed = (new Integer(3) * new Rational(2, 3)).Evaluate();
        Assert.Equal("2", mixed.ToString());
    }

    [Fact]
    public void Rational_ToBigReal_Precision()
    {
        var r = new Rational(1, 2);
        var br = r.ToBigReal(10);
        Assert.IsType<BigReal>(br);
        Assert.Equal(new BigInteger(10), br.Precision);
    }

    [Fact]
    public void Function_ToString_And_FunctionClass()
    {
        var f = new Function("f", new Symbol("x"), new Integer(2));
        Assert.Equal("f(x,2)", f.ToString());
    }

    [Fact]
    public void Plus_Evaluate_Grouping_And_Flattening()
    {
        var x = new Symbol("x");
        var plus = new Plus(new Plus(new Integer(new BigInteger(1)), new Plus(new Integer(new BigInteger(2)), x)), x);
        var eval = plus.Evaluate();
        var s = eval.ToString();
        // should contain numeric accumulation 3 and grouped 2*x
        Assert.Contains("3", s);
        Assert.Contains("(2*x)", s);
    }

    [Fact]
    public void Times_Evaluate_Distribution_And_ZeroShortCircuit()
    {
        // numeric product
        var t1 = new Times(new Integer(new BigInteger(2)), new Integer(new BigInteger(3))).Evaluate();
        Assert.Equal("6", t1.ToString());

        // zero short-circuit
        var tZero = new Times(new Integer(new BigInteger(5)), new Integer(new BigInteger(0))).Evaluate();
        Assert.Equal("0", tZero.ToString());

        // distribution: (2*x)^2 -> (4*(x^2)) after evaluate
        var dist = new Power(new Times(new Integer(new BigInteger(2)), new Symbol("x")), new Integer(new BigInteger(2))).Evaluate();
        Assert.Equal("(4*(x^2))", dist.ToString());
    }

    [Fact]
    public void Power_Evaluate_NumericAndNegativeExponent()
    {
        // integer power
        var p = new Power(new Integer(new BigInteger(2)), new Integer(new BigInteger(3))).Evaluate();
        Assert.Equal("8", p.ToString());

        // negative exponent -> rational
        var neg = new Power(new Integer(new BigInteger(2)), new Integer(new BigInteger(-1))).Evaluate();
        Assert.Equal("1/2", neg.ToString());

        // 0^0 -> Undefined
        var z0 = new Power(new Integer(new BigInteger(0)), new Integer(new BigInteger(0))).Evaluate();
        Assert.Equal("Undefined", z0.ToString());
    }

    [Fact]
    public void Simplifier_With_Operations_WorksAsExpected()
    {
        var addOp = new Plus(new Integer(new BigInteger(1)), new Integer(new BigInteger(2))).Evaluate();
        Assert.Equal("3", addOp.ToString());

        var mulOp = new Times(new Integer(new BigInteger(2)), new Integer(new BigInteger(3))).Evaluate();
        Assert.Equal("6", mulOp.ToString());

        // (20*x)*(2*x)^-1 -> 10 (use Function classes Times/Power)
        var expr = new Times(new Times(new Integer(new BigInteger(20)), new Symbol("x")),
            new Power(new Times(new Integer(new BigInteger(2)), new Symbol("x")), new Integer(new BigInteger(-1))));

        Assert.Equal("10", Simplifier.Simplify(expr).ToString());
    }

    [Fact]
    public void Comparer_And_EqualityComparer_Accessible()
    {
        // basic ToString ordering checks and structural equality via ToString
        var s1 = new Symbol("a");
        var s2 = new Symbol("b");
        Assert.True(string.Compare(s1.ToString(), s2.ToString(), StringComparison.Ordinal) < 0);

        var i1 = new Integer(new BigInteger(2));
        var i2 = new Integer(new BigInteger(2));
        Assert.Equal(i1.ToString(), i2.ToString());
    }

    // helper for debugging
    private void Dump(Expr e) => _output.WriteLine(e.ToString());
}
