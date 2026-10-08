using System;

namespace MathLibrary.Core;

public abstract class FunctionExpr : Expr
{
    public string Name { get; }

    public IReadOnlyList<Expr> Arguments { get; }

    public FunctionExpr(string name, params Expr[] arguments)
    {
        Name = name;
        Arguments = arguments;
    }

    public abstract FunctionExpr Create(params Expr[] arguments);

    public override abstract string ToString();
}

public sealed class Function : FunctionExpr
{
    public Function(string name, params Expr[] arguments) : base(name, arguments)
    {
    }

    public override FunctionExpr Create(params Expr[] arguments)
    {
        return new Function(Name, arguments);
    }

    public override string ToString()
    {
        return $"{Name}({string.Join(',', Arguments)})";
    }
}

public abstract class OneArgumentFunction : FunctionExpr
{
    public Expr Argument => Arguments[0];

    protected OneArgumentFunction(string name, Expr argument)
        : base(name, argument ?? throw new ArgumentNullException(nameof(argument))) { }
}
