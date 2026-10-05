using System;

namespace MathLibrary.Core;

public abstract class Function : Expr
{
    public string Name { get; }

    public IReadOnlyList<Expr> Arguments { get; }

    public Function(string name, params Expr[] arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public abstract Expr Simplify();

    public override string ToString()
    {
        return $"{Name}({string.Join(',', Arguments)})";
    }
}

public abstract class OneArgumentFunction : Function
{
    public Expr Argument => Arguments[0];

    protected OneArgumentFunction(string name, Expr argument)
        : base(name, argument ?? throw new ArgumentNullException(nameof(argument))) { }
}
