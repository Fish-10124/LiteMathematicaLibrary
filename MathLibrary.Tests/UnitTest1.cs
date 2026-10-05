using MathLibrary.Core;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Operations;
using MathLibrary.Core.Utils;
using Xunit.Abstractions;

namespace MathLibrary.Tests;

public class UnitTest1
{

    private readonly ITestOutputHelper _output;

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void Test1()
    {
        var expr = new Multiply(
            new Multiply(
                new Integer(20),
                new Symbol("x")
            ),
            new Power(
                new Multiply(
                    new Integer(2),
                    new Symbol("x")
                ), 
            new Integer(-1))
        );

        _output.WriteLine(Simplifier.Simplify(expr).ToString());
    }
}
