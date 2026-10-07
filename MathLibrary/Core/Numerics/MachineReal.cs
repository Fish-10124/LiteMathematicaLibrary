using System;

namespace MathLibrary.Core.Numerics;

public sealed class MachineReal : Real
{
    public override bool IsZero => Value == 0.0;

    public override bool IsPositiveOne => Value == 1.0;

    public override bool IsNegativeOne => Value == -1.0;

    public override bool IsPositive => Value > 0.0;

    public override bool IsNegative => Value < 0.0;

    public double Value { get; }

    public MachineReal(double value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public static implicit operator MachineReal(double value)
    {
        return new MachineReal(value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is MachineReal other) return Value.Equals(other.Value);
        return false;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
