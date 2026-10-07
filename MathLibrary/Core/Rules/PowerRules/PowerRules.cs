using System;

namespace MathLibrary.Core.Rules.PowerRules;

public abstract class PowerRules : IRules
{
    public abstract Expr Apply(Expr expr);

    public abstract bool Match(Expr expr);
}
