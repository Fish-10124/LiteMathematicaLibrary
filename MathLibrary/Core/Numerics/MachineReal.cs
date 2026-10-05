using System;

namespace MathLibrary.Core.Numerics;

public sealed class MachineReal : Real
{
    public double Value { get; }

    public MachineReal(double value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
