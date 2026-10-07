using MathLibrary.Core.Numerics;

namespace MathLibrary.Core.Types;

public sealed class Term
{
    public NumberExpr Coefficient;

    public Expr Kernel;

    public override bool Equals(object? obj)
    {
        if (obj is not Term term) return false;
        return Equals(this.Coefficient, term.Coefficient) 
                && Equals(this.Kernel, term.Kernel);
    }
}
