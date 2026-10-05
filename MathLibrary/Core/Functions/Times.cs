using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Functions;

public sealed class Times : Function
{
    public Times(params Expr[] factors) : base(nameof(Times), factors) { }
}
