using System;

namespace MathLibrary.Core.Numerics;

public abstract class Real : NumberExpr
{
    public override NumberKind Kind => NumberKind.Real;
}
