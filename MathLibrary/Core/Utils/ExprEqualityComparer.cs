using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary.Core.Utils
{
    public sealed class ExprEqualityComparer : IEqualityComparer<Expr>
    {
        public static ExprEqualityComparer Instance { get; } = new ExprEqualityComparer();

        public bool Equals(Expr? x, Expr? y)
        {
            return ExprComparer.Instance.Compare(x, y) == 0;
        }

        public int GetHashCode([DisallowNull] Expr obj)
        {
            return obj.ToString().GetHashCode();
        }
    }
}
