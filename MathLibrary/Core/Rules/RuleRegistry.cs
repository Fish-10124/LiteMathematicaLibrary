using System;
using MathLibrary.Core.Functions;

namespace MathLibrary.Core.Rules;

public static class RuleRegistry
{
    private static readonly ExpandRules.ExpandRules[] ExpandRules = new ExpandRules.ExpandRules[]
    {
        new ExpandRules.PowerOfSumRule(),
        new ExpandRules.TimesOverPlusRule(),
        new ExpandRules.NestedExpandRule()
    };

    private static readonly PlusRules.PlusRules[] PlusRules = new PlusRules.PlusRules[]
    {
        new PlusRules.NormativeStructure(),
        new PlusRules.SpecialSymbolsPropagation(),
        new PlusRules.ConstantFolding(),
        new PlusRules.TermsCollecting()
    };

    private static readonly TimesRules.TimesRules[] TimesRules = new TimesRules.TimesRules[]
    {
        new TimesRules.NormativeStructure(),
        new TimesRules.SpecialSymbolsPropagation(),
        new TimesRules.ConstantFolding(),
        new TimesRules.TermsCollecting()
    };

    private static readonly PowerRules.PowerRules[] PowerRules = new PowerRules.PowerRules[]
    {
        new PowerRules.NormativeStructure(),
        new PowerRules.SpecialSymbolsPropagation(),
        new PowerRules.ConstantFolding()
    };

    public static IEnumerable<ExpandRules.ExpandRules> GetExpandRules()
    {
        return ExpandRules;
    }

    public static IEnumerable<IRules> GetSimplifyRules(Expr expr)
    {
        switch (expr)
        {
            case Plus:
                return PlusRules;
            case Times:
                return TimesRules;
            case Power:
                return PowerRules;
            default:
                return Enumerable.Empty<IRules>();
        }
    }
}
