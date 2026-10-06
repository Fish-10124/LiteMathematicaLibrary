using System;

namespace MathLibrary.Core;

public class Symbol : Expr
{
    public string Name { get; }

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

    public override bool Equals(object? obj)
    {
        if (obj is Symbol other)
        {
            return this.Name == other.Name;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public static implicit operator Symbol(string name)
    {
        return new Symbol(name);
    }
}
