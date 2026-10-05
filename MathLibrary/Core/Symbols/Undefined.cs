using System;

namespace MathLibrary.Core.Symbols;

public sealed class Undefined : Expr
{
    public static Undefined Instance { get; } = new Undefined();

    private Undefined() {}

    public override string ToString()
    {
        return "Undefined";
    }
}
