using System;
using System.Numerics;

namespace MathLibrary.Core.Numerics;

public sealed class Integer : NumberExpr
{
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