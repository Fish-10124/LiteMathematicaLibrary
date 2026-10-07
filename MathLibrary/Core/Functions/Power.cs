using MathLibrary.Core.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Functions;

public sealed class Power : FunctionExpr
{
    public Expr Base => Arguments[0];
    public Expr Exponent => Arguments[1];

    public Power(Expr @base, Expr exponent) : base(nameof(Power), @base, exponent) { }

    public override string ToString()
    {
        return $"({Base}^{Exponent})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Power other) return Equals(Base, other.Base) && Equals(Exponent, other.Exponent);
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Base, Exponent);
    }
}

