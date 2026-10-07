using System;

namespace MathLibrary.Core.Rules.TimesRules;

public abstract class TimesRules : IRules
{
    public abstract Expr Apply(Expr expr);

    public abstract bool Match(Expr expr);
}
