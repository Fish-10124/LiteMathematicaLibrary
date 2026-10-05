using System;

namespace MathLibrary.Core.Operations;

public sealed class Add : Expr
{
    public Expr Left { get; }

    public Expr Right { get; }

    public Add(Expr left, Expr right)
    {
        Left = left;
        Right = right;
    }

    public override string ToString()
    {
        return $"({Left}+{Right})";
    }
}
