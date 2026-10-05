using MathLibrary.Core;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Operations;
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
        var expr = new Power(
            new Symbol("x"),
            new Integer(2)
        );

        _output.WriteLine(expr.ToString());
    }
}
