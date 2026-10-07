using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;
using System;
using System.Numerics;

namespace MathLibrary.Core;

public abstract class Expr
{
    public virtual BigInteger LeafCount => 1;

    public abstract override string ToString();

    public static Expr operator +(Expr left, Expr right)
    {
        return new Plus(left, right);
    }

    public static Expr operator -(Expr left, Expr right)
    {
        return new Plus(left, new Times(new Integer(-1), right));
    }

    public static Expr operator *(Expr left, Expr right)
    {
        return new Times(left, right);
    }

    public static Expr operator /(Expr left, Expr right)
    {
        return new Times(left, new Power(right, new Integer(-1)));
    }

    public static Expr operator ^(Expr left, Expr right)
    {
        return new Power(left, right);
    }

    public static implicit operator Expr(int value)
    {
        return new Integer(value);
    }

    public static implicit operator Expr(double value)
    {
        return new MachineReal(value);
    }
}
