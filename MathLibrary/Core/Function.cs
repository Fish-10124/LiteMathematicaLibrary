using System;

namespace MathLibrary.Core;

public sealed class Function : Expr
{
    public string Name { get; }

    public IReadOnlyList<Expr> Arguments { get; }

    public Function(string name, params Expr[] arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public override string ToString()
    {
        return $"{Name}({string.Join(',', Arguments)})";
    }
}
