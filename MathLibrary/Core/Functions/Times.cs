using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Functions;

public sealed class Times : FunctionExpr
{
    public IReadOnlyList<Expr> Factors => this.Arguments;

    public Times(params Expr[] factors) : base(nameof(Times), factors.OrderBy(x => x, ExpressionComparer.Instance).ToArray()) { }

    public override string ToString()
    {
        return $"({string.Join("*", this.Factors)})";
    }

    public override bool Equals(object? obj)
    {
        return obj is Times other && Factors.SequenceEqual(other.Factors);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var factor in Factors)
        {
            hash.Add(factor);
        }
        return hash.ToHashCode();
    }
}
