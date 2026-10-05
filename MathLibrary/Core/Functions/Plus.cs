using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Functions;

public sealed class Plus : Function
{
    public Plus(params Expr[] terms) : base(nameof(Plus), terms) { }

    
}
