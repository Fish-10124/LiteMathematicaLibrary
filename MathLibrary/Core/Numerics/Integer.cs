using System;
using System.Numerics;

namespace MathLibrary.Core.Numerics;

public sealed class Integer : NumberExpr
{
    public override bool IsZero => Value.IsZero;

    public override bool IsPositiveOne => Value.IsOne;

    public override bool IsNegativeOne => Value == -1;

    public override bool IsPositive => Value.Sign > 0;

    public override bool IsNegative => Value.Sign < 0;

    public BigInteger Value { get; set; }

    public Integer(BigInteger value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}