using System;

namespace MathLibrary.Core.Numerics;

public abstract class NumberExpr : Expr
{
    public virtual bool IsZero => false;

    public virtual bool IsPositiveOne => false;

    public virtual bool IsNegativeOne => false;

    public virtual bool IsPositive => false;

    public virtual bool IsNegative => false;
}
