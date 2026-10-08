using MathLibrary.Core.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Functions;

public sealed class Plus : FunctionExpr
{
    public IReadOnlyList<Expr> Terms => this.Arguments;

    public Plus(params Expr[] terms) : base(nameof(Plus), terms.OrderBy(x => x, ExpressionComparer.Instance).ToArray()) { }

    public override FunctionExpr Create(params Expr[] arguments)
    {
        return new Plus(arguments);
    }

    public override string ToString()
    {
        return $"({string.Join("+", this.Terms)})";
    }

    public override bool Equals(object? obj)
    {
        return obj is Plus other && Terms.SequenceEqual(other.Terms);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var term in Terms)
        {
            hash.Add(term);
        }
        return hash.ToHashCode();
    }
}
