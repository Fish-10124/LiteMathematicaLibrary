using System;
using System.Numerics;

namespace MathLibrary.Core.Numerics;

public abstract class NumberExpr : Expr
{
    public virtual bool IsZero => false;

    public virtual bool IsPositiveOne => false;

    public virtual bool IsNegativeOne => false;

    public virtual bool IsPositive => false;

    public virtual bool IsNegative => false;

    public abstract NumberKind Kind { get; }
}
