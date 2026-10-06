using MathLibrary.Core;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Tests;

public class SymbolsOperationTests
{
    private static readonly Integer Zero = 0;
    private static readonly Integer One = 1;
    private static readonly Integer Two = 2;
    private static readonly Integer NegOne = -1;
    private static readonly Symbol X = "x";

    #region 1. Plus (加法运算测试)

    [Fact]
    public void Plus_WithUndefined_ReturnsUndefined()
    {
        // 任何包含 Undefined 的加法结果均为 Undefined
        Assert.Same(Symbols.Undefined, (Symbols.Undefined + 1).Evaluate());
        Assert.Same(Symbols.Undefined, new Plus(X, Symbols.Undefined).Evaluate());
        Assert.Same(Symbols.Undefined, new Plus(Symbols.Undefined, Symbols.Indeterminate).Evaluate());
        Assert.Same(Symbols.Undefined, new Plus(Symbols.Undefined, Symbols.ComplexInfinity).Evaluate());
    }

    [Fact]
    public void Plus_WithIndeterminate_ReturnsIndeterminate()
    {
        // 包含 Indeterminate (且无 Undefined) 的加法结果均为 Indeterminate
        Assert.Same(Symbols.Indeterminate, new Plus(Symbols.Indeterminate, One).Evaluate());
        Assert.Same(Symbols.Indeterminate, new Plus(X, Symbols.Indeterminate).Evaluate());
        Assert.Same(Symbols.Indeterminate, new Plus(Symbols.Indeterminate, Symbols.ComplexInfinity).Evaluate());
    }

    [Fact]
    public void Plus_WithComplexInfinity_ReturnsExpected()
    {
        // 单个 ComplexInfinity 与普通项相加 -> ComplexInfinity
        Assert.Same(Symbols.ComplexInfinity, new Plus(Symbols.ComplexInfinity, One).Evaluate());
        Assert.Same(Symbols.ComplexInfinity, new Plus(X, Symbols.ComplexInfinity).Evaluate());

        // 多个 ComplexInfinity 相加 -> Indeterminate
        Assert.Same(Symbols.Indeterminate, new Plus(Symbols.ComplexInfinity, Symbols.ComplexInfinity).Evaluate());
        Assert.Same(Symbols.Indeterminate, new Plus(Symbols.ComplexInfinity, Symbols.ComplexInfinity, One).Evaluate());
    }

    #endregion

    #region 2. Times (乘法运算测试)

    [Fact]
    public void Times_WithUndefined_ReturnsUndefined()
    {
        // 任何包含 Undefined 的乘法结果均为 Undefined
        Assert.Same(Symbols.Undefined, new Times(Symbols.Undefined, One).Evaluate());
        Assert.Same(Symbols.Undefined, new Times(Zero, Symbols.Undefined).Evaluate());
        Assert.Same(Symbols.Undefined, new Times(Symbols.Undefined, Symbols.ComplexInfinity).Evaluate());
    }

    [Fact]
    public void Times_WithIndeterminate_ReturnsIndeterminate()
    {
        // 包含 Indeterminate (且无 Undefined) 的乘法结果均为 Indeterminate
        Assert.Same(Symbols.Indeterminate, new Times(Symbols.Indeterminate, Two).Evaluate());
        Assert.Same(Symbols.Indeterminate, new Times(Zero, Symbols.Indeterminate).Evaluate());
        Assert.Same(Symbols.Indeterminate, new Times(Symbols.Indeterminate, Symbols.ComplexInfinity).Evaluate());
    }

    [Fact]
    public void Times_WithComplexInfinity_ReturnsExpected()
    {
        // ComplexInfinity * 0 -> Indeterminate
        Assert.Same(Symbols.Indeterminate, new Times(Symbols.ComplexInfinity, Zero).Evaluate());

        // ComplexInfinity * 非零项 -> ComplexInfinity
        Assert.Same(Symbols.ComplexInfinity, new Times(Symbols.ComplexInfinity, Two).Evaluate());
        Assert.Same(Symbols.ComplexInfinity, new Times(Symbols.ComplexInfinity, X).Evaluate());
        Assert.Same(Symbols.ComplexInfinity, new Times(Symbols.ComplexInfinity, Symbols.ComplexInfinity).Evaluate());
    }

    #endregion

    #region 3. Power (乘方运算测试)

    [Fact]
    public void Power_WithUndefinedOrIndeterminate_ReturnsExpected()
    {
        // 底数或指数包含 Undefined -> Undefined
        Assert.Same(Symbols.Undefined, new Power(Symbols.Undefined, Two).Evaluate());
        Assert.Same(Symbols.Undefined, new Power(Two, Symbols.Undefined).Evaluate());

        // 底数或指数包含 Indeterminate -> Indeterminate
        Assert.Same(Symbols.Indeterminate, new Power(Symbols.Indeterminate, Two).Evaluate());
        Assert.Same(Symbols.Indeterminate, new Power(Two, Symbols.Indeterminate).Evaluate());
    }

    [Fact]
    public void Power_SpecialIndeterminateRules_ReturnsIndeterminate()
    {
        // 0^0 -> Indeterminate
        Assert.Same(Symbols.Indeterminate, new Power(Zero, Zero).Evaluate());

        // ComplexInfinity^0 -> Indeterminate
        Assert.Same(Symbols.Indeterminate, new Power(Symbols.ComplexInfinity, Zero).Evaluate());
    }

    [Fact]
    public void Power_WithComplexInfinity_ReturnsExpected()
    {
        // 0 ^ 负数 -> ComplexInfinity
        Assert.Same(Symbols.ComplexInfinity, new Power(Zero, NegOne).Evaluate());

        // ComplexInfinity ^ 负数 -> 0
        var infPowerNeg = new Power(Symbols.ComplexInfinity, NegOne).Evaluate();
        Assert.IsType<Integer>(infPowerNeg);
        Assert.True(((Integer)infPowerNeg).IsZero);
    }

    [Fact]
    public void Power_IdentityRules_ReturnsExpected()
    {
        // x^0 -> 1 (非特殊值时)
        var powerZero = new Power(Two, Zero).Evaluate();
        Assert.IsType<Integer>(powerZero);
        Assert.True(((Integer)powerZero).IsPositiveOne);

        // 1^x -> 1
        var powerBaseOne = new Power(One, X).Evaluate();
        Assert.IsType<Integer>(powerBaseOne);
        Assert.True(((Integer)powerBaseOne).IsPositiveOne);

        // x^1 -> x
        var powerExpOne = new Power(X, One).Evaluate();
        Assert.Same(X, powerExpOne);

        // 0^正数 -> 0
        var zeroPowerPos = new Power(Zero, Two).Evaluate();
        Assert.IsType<Integer>(zeroPowerPos);
        Assert.True(((Integer)zeroPowerPos).IsZero);
    }

    #endregion
}