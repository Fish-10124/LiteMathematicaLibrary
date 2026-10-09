using System;

namespace MathLibrary.Core.Rules.ExpandRules;

public abstract class ExpandRules : IRules
{
    public abstract Expr Apply(Expr expr);

    public abstract bool Match(Expr expr);
}
