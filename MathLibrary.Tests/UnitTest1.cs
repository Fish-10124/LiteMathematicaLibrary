using MathLibrary.Core;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Functions;
using Xunit.Abstractions;

namespace MathLibrary.Tests;

public class UnitTest1
{
    private readonly ITestOutputHelper _output;

    private static readonly Symbol X = "x";
    private static readonly Symbol Y = "y";
    private static readonly Symbol A = "a";
    private static readonly Symbol B = "b";
    private static readonly Symbol C = "c";

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void PolynomialSimplifyTest()
    {
        var expr1 = 3 * (X ^ 2) * Y - 2 * (X * (Y ^ 2) - 2 * ((X ^ 2) * Y - 2 * X * (Y ^ 2))) - 4 * (X ^ 2) * Y;
        _output.WriteLine(Simplifier.Simplify(expr1).ToString());

        var expr2 = 2 * A * (3 * A - B + 2 * C) - 3 * B * (A - 2 * B - C) - 4 * C * (A + B - 3 * C);
        _output.WriteLine(Simplifier.Simplify(expr2).ToString());

        var expr3 = (2 * X - 3 * Y) * (X + 4 * Y) - 2 * ((X - 2 * Y) ^ 2) + (3 * X + Y) * (3 * X - Y);
        _output.WriteLine(Simplifier.Simplify(expr3).ToString());

        var expr4 = ((X ^ 2) * Y) / 2 - 3 * ((X * (Y ^ 2)) / 3 - ((X ^ 2) * Y) / 6) + 2 * X * Y * ((3 * X) / 4 - (Y / 2));
        _output.WriteLine(Simplifier.Simplify(expr4).ToString());

        var expr5 = ((A + B) ^ 3) - A * (A - 2 * B) * (A + 2 * B) - 3 * A * B * (A + B);
        _output.WriteLine(Simplifier.Simplify(expr5).ToString());
    }

    [Fact]
    public void NumberOperationText()
    {
        var expr1 = ((new Integer(3) * new Integer(5) + new Integer(2)) ^ new Integer(3)) / new Integer(17);
        _output.WriteLine(Simplifier.Simplify(expr1).ToString());

        var expr2 = new Integer(17) ^ new Integer(3);
        _output.WriteLine(Simplifier.Simplify(expr2).ToString());

        var expr3 = (new Integer(0) ^ new Integer(-5)) + new Integer(5);
        _output.WriteLine(Simplifier.Simplify(expr3).ToString());

        var expr4 = A + 3 + 5 * A;
        _output.WriteLine(Simplifier.Simplify(expr4).ToString());

        var expr5 = (X ^ 2) * (X ^ 3);
        _output.WriteLine(Simplifier.Simplify(expr5).ToString());

        Assert.True(Simplifier.Simplify(X * Y).Equals(Simplifier.Simplify(Y * X)));

        Assert.True(Simplifier.Simplify(X * X * X).Equals(new Power(X,new Integer(3))));

        Assert.True(Simplifier.Simplify((X ^ 2) * X * (X ^ 3)).Equals(X ^ 6));

        _output.WriteLine(Simplifier.Simplify(2 * X + 3 * X).ToString());
        _output.WriteLine(Simplifier.Simplify(2 * (X ^ 2) + 3 * (X ^ 2)).ToString());
    }

    // helper for debugging
    private void Dump(Expr e) => _output.WriteLine(e.ToString());
}
