using System;

namespace MathLibrary.Core;

public class Symbol : Expr
{
    private string Name { get; }

    public Symbol(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Symbol name cannot be empty.", nameof(name));
        }
        
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}
