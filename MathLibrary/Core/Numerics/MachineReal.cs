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

    public override NumberRank Rank => NumberRank.Real;

    public MachineReal(double value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override NumberExpr ToNextRank()
    {
        return new Complex(this, new MachineReal(0.0));
    }

    protected override NumberExpr AddSameType(NumberExpr expr)
    {
        var same = expr as MachineReal ?? throw new ArgumentException("Argument is not the same type", nameof(expr));
        return new MachineReal(this.Value + same.Value);
    }

    protected override NumberExpr MultiplySameType(NumberExpr expr)
    {
        var same = expr as MachineReal ?? throw new ArgumentException("Argument is not the same type", nameof(expr));
        return new MachineReal(this.Value * same.Value);
    }
}
