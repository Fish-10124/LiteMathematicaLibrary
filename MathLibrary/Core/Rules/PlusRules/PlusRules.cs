using System;

namespace MathLibrary.Core.Rules.PlusRules;

public abstract class PlusRules : IRules
{
    public abstract Expr Apply(Expr expr);

    public abstract bool Match(Expr expr);
}