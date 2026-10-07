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

    public override NumberKind Kind => NumberKind.Integer;

    public Integer(BigInteger value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator Integer(int value)
    {
        return new Integer(value);
    }

    public static implicit operator Integer(long value)
    {
        return new Integer(value);
    }

    public static implicit operator Integer(BigInteger value)
    {
        return new Integer(value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is Integer other) return this.Value == other.Value;
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}