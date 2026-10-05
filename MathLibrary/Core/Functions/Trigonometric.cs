using System;

namespace MathLibrary.Core.Functions;

public sealed class Sin : OneArgumentFunction
{
    public Sin(Expr argument) : base(nameof(Sin), argument) {}
}

public sealed class Cos : OneArgumentFunction
{
    public Cos(Expr argument) : base(nameof(Cos), argument) { }
}

public sealed class Tan : OneArgumentFunction
{
    public Tan(Expr argument) : base(nameof(Tan), argument) { }
}

public sealed class Cot : OneArgumentFunction
{
    public Cot(Expr argument) : base(nameof(Cot), argument) { }
}

public sealed class Sec : OneArgumentFunction
{
    public Sec(Expr argument) : base(nameof(Sec), argument) { }
}

public sealed class Csc : OneArgumentFunction
{
    public Csc(Expr argument) : base(nameof(Csc), argument) { }
}

public sealed class ArcSin : OneArgumentFunction
{
    public ArcSin(Expr argument) : base(nameof(ArcSin), argument) { }
}

public sealed class ArcCos : OneArgumentFunction
{
    public ArcCos(Expr argument) : base(nameof(ArcCos), argument) { }
}

public sealed class ArcTan : OneArgumentFunction
{
    public ArcTan(Expr argument) : base(nameof(ArcTan), argument) { }
}

public sealed class ArcCot : OneArgumentFunction
{
    public ArcCot(Expr argument) : base(nameof(ArcCot), argument) { }
}

public sealed class ArcSec : OneArgumentFunction
{
    public ArcSec(Expr argument) : base(nameof(ArcSec), argument) { }
}

public sealed class ArcCsc : OneArgumentFunction
{
    public ArcCsc(Expr argument) : base(nameof(ArcCsc), argument) { }
}
