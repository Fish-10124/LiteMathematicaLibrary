using System;

namespace MathLibrary.Core.Rules;

public interface IRules
{
    bool Match(Expr expr);
    Expr Apply(Expr expr);
}
