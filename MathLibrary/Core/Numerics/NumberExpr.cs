using System;

namespace MathLibrary.Core.Numerics;

public abstract class NumberExpr : Expr
{
    public virtual bool IsZero => false;

    public virtual bool IsPositiveOne => false;

    public virtual bool IsNegativeOne => false;

    public virtual bool IsPositive => false;

    public virtual bool IsNegative => false;

    public abstract NumberRank Rank { get; }

    public abstract NumberExpr ToNextRank();

    public NumberExpr ConvertToRank(NumberRank target)
    {
        var current = this;
        while (current.Rank < target)
        {
            current = current.ToNextRank();
        }
        return current;
    }

    public NumberExpr Add(NumberExpr other)
    {
        if (this.Rank == other.Rank)
        {
            return AddSameType(other);
        }

        if (this.Rank < other.Rank) {
            return this.ConvertToRank(other.Rank).Add(other);
        }

        return this.Add(other.ConvertToRank(this.Rank));
    }

    public NumberExpr Multiply(NumberExpr other)
    {
        if (this.Rank == other.Rank)
        {
            return MultiplySameType(other);
        }

        if (this.Rank < other.Rank)
        {
            return this.ConvertToRank(other.Rank).Multiply(other);
        }

        return this.Multiply(other.ConvertToRank(this.Rank));
    }

    protected abstract NumberExpr AddSameType(NumberExpr expr);

    protected abstract NumberExpr MultiplySameType(NumberExpr expr);
}
