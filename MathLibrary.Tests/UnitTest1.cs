using MathLibrary.Core;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Operations;
using MathLibrary.Core.Utils;
using Xunit.Abstractions;
using Xunit;
using BigInteger = System.Numerics.BigInteger;
using MathLibrary.Core.Functions;

namespace MathLibrary.Tests;

public class UnitTest1
{

    private readonly ITestOutputHelper _output;

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void IntegerAndRational_BasicBehavior()
    {
        var i = new Integer(new BigInteger(6));
        Assert.Equal("6", i.ToString());

        var r = new Rational(4, 6); // reduced to 2/3
        Assert.Equal("2/3", r.ToString());

        Assert.Throws<DivideByZeroException>(() => new Rational(1, 0));
    }

    [Fact]
    public void RealAndComplex_ToString()
    {
        var m = new MachineReal(3.14);
        Assert.Equal("3.14", m.ToString());

        var b = new BigReal(new BigInteger(123), new BigInteger(-2), new BigInteger(10));
        Assert.Equal("123e-2", b.ToString());

        var c = new Complex(new Integer(1), new Integer(0));
        Assert.Equal("(1+0i)", c.ToString());
    }

    [Fact]
    public void SymbolAndFunction_ToStringAndValidation()
    {
        var s = new Symbol("x");
        Assert.Equal("x", s.ToString());

        Assert.Throws<ArgumentException>(() => new Symbol("  "));

        var f = new Function("f", new Symbol("x"), new Integer(2));
        Assert.Equal("f(x,2)", f.ToString());
    }

    [Fact]
    public void Operation_ToString()
    {
        var add = new Plus(new Integer(1), new Symbol("x"));
        Assert.Equal("(1+x)", add.ToString());

        var mul = new Times(new Integer(2), new Symbol("y"));
        Assert.Equal("(2*y)", mul.ToString());

        var pow = new Power(new Symbol("x"), new Integer(3));
        Assert.Equal("(x^3)", pow.ToString());
    }

    [Fact]
    public void Simplifier_AddAndMultiply_SimpleCases()
    {
        // 1 + 2 -> 3
        var a = new Integer(1) + new Integer(2);
        Assert.Equal("3", Simplifier.Simplify(a).ToString());

        // 2 * 3 -> 6
        var m = new Integer(2) * new Integer(3);
        Assert.Equal("6", Simplifier.Simplify(m).ToString());

        // any * 0 -> 0
        var mz = new Times(new Integer(5), new Integer(0));
        Assert.Equal("0", Simplifier.Simplify(mz).ToString());
    }

    [Fact]
    public void Simplifier_PowerEdgeCases()
    {
        // 0^0 -> Undefined
        var z0 = new Power(new Integer(0), new Integer(0));
        Assert.Equal("Undefined", Simplifier.Simplify(z0).ToString());

        // x^0 -> 1
        var x0 = new Power(new Symbol("x"), new Integer(0));
        Assert.Equal("1", Simplifier.Simplify(x0).ToString());

        // 0^n -> 0
        var zeroPowN = new Power(new Integer(0), new Integer(3));
        Assert.Equal("0", Simplifier.Simplify(zeroPowN).ToString());

        // 2^3 -> 8
        var p = new Power(new Integer(2), new Integer(3));
        Assert.Equal("8", Simplifier.Simplify(p).ToString());
    }

    [Fact]
    public void Simplifier_CollectAndCancelFactors()
    {
        // (20*x) * (2*x)^-1 -> 10
        var expr = new Times(
            new Times(
                new Integer(20),
                new Symbol("x")
            ),
            new Power(
                new Times(
                    new Integer(2),
                    new Symbol("x")
                ),
                new Integer(-1))
        );

        Assert.Equal("10", Simplifier.Simplify(expr).ToString());
    }

    [Fact]
    public void Simplifier_AddGrouping()
    {
        // x + x + 1 -> (1+(2*x))
        var expr = new Plus(new Plus(new Symbol("x"), new Symbol("x")), new Integer(1));
        var simplified = Simplifier.Simplify(expr).ToString();
        Assert.Equal("(1+(2*x))", simplified);
    }

    // Helper: demonstrate writing output when debugging tests
    private void Dump(Expr e) => _output.WriteLine(e.ToString());
}
