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

    public override NumberRank Rank => NumberRank.Integer;

    public Integer(BigInteger value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override NumberExpr ToNextRank()
    {
        return new Rational(Value, 1);
    }

    protected override NumberExpr AddSameType(NumberExpr expr)
    {
        var same = expr as Integer ?? throw new ArgumentException("Argument is not the same type", nameof(expr));
        return new Integer(this.Value + same.Value);
    }

    protected override NumberExpr MultiplySameType(NumberExpr expr)
    {
        var same = expr as Integer ?? throw new ArgumentException("Argument is not the same type", nameof(expr));
        return new Integer(this.Value * same.Value);
    }
}