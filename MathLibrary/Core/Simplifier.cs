using System;
using System.Numerics;
using MathLibrary.Core.Functions;
using MathLibrary.Core.Numerics;
using MathLibrary.Core.Rules;

namespace MathLibrary.Core;

public static class Simplifier
{

    private static readonly List<IRules> Rules = new List<IRules>
    {
        new Rules.PowerRules.NormativeStructure(),
        new Rules.PowerRules.ConstantFolding(),

        new Rules.TimesRules.NormativeStructure(),
        new Rules.TimesRules.ConstantFolding(),
        new Rules.TimesRules.TermsCollecting(),

        new Rules.PlusRules.NormativeStructure(),
        new Rules.PlusRules.ConstantFolding(),
        new Rules.PlusRules.TermsCollecting()
    };

    public static Expr Simplify(Expr expr)
    {
        ArgumentNullException.ThrowIfNull(expr);

        if (expr is null) return null!;

        // ==========================================
        // 1. 【递】：先递归处理子节点，并返回给上层
        // ==========================================
        Expr simplifiedNode = expr switch
        {
            // 递归去算 Plus 的每一项，算完重新组合
            Plus plus => new Plus(plus.Terms.Select(Simplify).ToArray()),

            // 递归去算 Times 的每一个因子，算完重新组合
            Times times => new Times(times.Factors.Select(Simplify).ToArray()),

            // 递归去算 Base 和 Exponent，算完重新组合
            Power power => new Power(Simplify(power.Base), Simplify(power.Exponent)),

            // 叶子节点（数字、变量），不需要再“递”了，原样返回
            _ => expr
        };

        // ==========================================
        // 2. 【归】：子节点全算好了，回到当前父节点应用规则
        // ==========================================
        bool changed;
        do
        {
            changed = false;
            foreach (var rule in Rules)
            {
                if (rule.Match(simplifiedNode))
                {
                    Expr next = rule.Apply(simplifiedNode);
                    if (!Equals(next, simplifiedNode))
                    {
                        // 如果规则修改了结构（比如展开了括号），
                        // 就对产生的新结构重新递归 Simplify 一次
                        simplifiedNode = Simplify(next);
                        changed = true;
                        break;
                    }
                }
            }
        } while (changed);

        // 最终把化简彻底的当前节点，返回给“它的上一个（父）节点”
        return simplifiedNode;
    }
}
