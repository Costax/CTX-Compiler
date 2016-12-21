using System.ComponentModel;

namespace CTX_LexicalAnalyzer
{
    public enum LexicCategories
    {
        [Description("@")]
        Declaratory,
        [Description(",")]
        Separator,
        [Description("<")]
        LessThan,
        [Description(">")]
        MoreThan,
        [Description(":")]
        Equals,
        [Description(">:")]
        MoreEqual,
        [Description("<:")]
        LessEqual,
        [Description("#")]
        Different,
        [Description("{")]
        StartProcess,
        [Description("}")]
        EndProcess,
        [Description("(")]
        StartArithProcess,
        [Description(")")]
        EndArithProcess,
        [Description("[")]
        StartParameter,
        [Description("]")]
        EndParameter,
        [Description("++")]
        Incremental,
        [Description("--")]
        Decremental,
        [Description("+")]
        Plus,
        [Description("-")]
        Minus,
        [Description("*")]
        Product,
        [Description("/")]
        Division,
        [Description("^")]
        Power,
        [Description("frag")]
        frag,
        [Description("speck")]
        speck,
        [Description("sequ")]
        sequ,
        [Description("splint")]
        splint,
        [Description("bin")]
        bin,
        [Description("phase")]
        phase,
        [Description("sprint")]
        sprint,
        [Description("Prime")]
        Prime,
        [Description("inc")]
        inc,
        [Description("but")]
        but,
        [Description(";")]
        Terminator,
        [Description("%")]
        Module
    }

    public static class LexicCategoryExtensions
    {
        public static string Category (this LexicCategories self)
        {
            switch (self)
            {
                case LexicCategories.Declaratory:
                    return "Declaratory";
                case LexicCategories.Decremental:
                    return "Arithmetic Op";
                case LexicCategories.Division:
                    return "Arithmetic Op";
                case LexicCategories.Plus:
                    return "Arithmetic Op";
                case LexicCategories.Minus:
                    return "Arithmetic Op";
                case LexicCategories.Product:
                    return "Arithmetic Op";
                case LexicCategories.Power:
                    return "Arithmetic Op";
                case LexicCategories.Incremental:
                    return "Arithmetic Op";
                case LexicCategories.Module:
                    return "Arithmetic Op";
                case LexicCategories.Terminator:
                    return "Terminator";
                case LexicCategories.frag:
                    return "Reserved word";
                case LexicCategories.speck:
                    return "Reserved word";
                case LexicCategories.sequ:
                    return "Reserved word";
                case LexicCategories.splint:
                    return "Reserved word";
                case LexicCategories.bin:
                    return "Reserved word";
                case LexicCategories.phase:
                    return "Reserved word";
                case LexicCategories.sprint:
                    return "Reserved word";
                case LexicCategories.Prime:
                    return "Reserved word";
                case LexicCategories.inc:
                    return "Reserved word";
                case LexicCategories.but:
                    return "Reserved word";
                case LexicCategories.MoreThan:
                    return "Comparator";
                case LexicCategories.LessThan:
                    return "Comparator";
                case LexicCategories.Equals:
                    return "Comparator";
                case LexicCategories.MoreEqual:
                    return "Comparator";
                case LexicCategories.LessEqual:
                    return "Comparator";
                case LexicCategories.Different:
                    return "Comparator";
                case LexicCategories.Separator:
                    return "Separator";
                case LexicCategories.StartProcess:
                    return "Container";
                case LexicCategories.EndProcess:
                    return "Container";
                case LexicCategories.StartArithProcess:
                    return "Container";
                case LexicCategories.EndArithProcess:
                    return "Container";
                case LexicCategories.StartParameter:
                    return "Container";
                case LexicCategories.EndParameter:
                    return "Container";
                default:
                    return null;
            }
        }

        public static int Key(this LexicCategories self)
        {
            switch (self)
            {
                case LexicCategories.Declaratory:
                    return 1;
                case LexicCategories.Decremental:
                    return 2;
                case LexicCategories.Division:
                    return 2;
                case LexicCategories.Plus:
                    return 2;
                case LexicCategories.Minus:
                    return 2;
                case LexicCategories.Product:
                    return 2;
                case LexicCategories.Power:
                    return 2;
                case LexicCategories.Incremental:
                    return 2;
                case LexicCategories.Terminator:
                    return 3;
                case LexicCategories.frag:
                    return 4;
                case LexicCategories.speck:
                    return 4;
                case LexicCategories.sequ:
                    return 4;
                case LexicCategories.splint:
                    return 4;
                case LexicCategories.bin:
                    return 4;
                case LexicCategories.phase:
                    return 4;
                case LexicCategories.sprint:
                    return 4;
                case LexicCategories.Prime:
                    return 4;
                case LexicCategories.inc:
                    return 4;
                case LexicCategories.but:
                    return 4;
                case LexicCategories.MoreThan:
                    return 5;
                case LexicCategories.LessThan:
                    return 5;
                case LexicCategories.Equals:
                    return 5;
                case LexicCategories.MoreEqual:
                    return 5;
                case LexicCategories.LessEqual:
                    return 5;
                case LexicCategories.Different:
                    return 5;
                case LexicCategories.Separator:
                    return 6;
                case LexicCategories.StartProcess:
                    return 7;
                case LexicCategories.EndProcess:
                    return 7;
                case LexicCategories.StartArithProcess:
                    return 7;
                case LexicCategories.EndArithProcess:
                    return 7;
                case LexicCategories.StartParameter:
                    return 7;
                case LexicCategories.EndParameter:
                    return 7;
                default:
                    return 0;
            }
        }

        public static string GetValidType(this LexicCategories self)
        {
            switch (self)
            {
                case LexicCategories.frag:
                    return "char";
                case LexicCategories.speck:
                    return "int";
                case LexicCategories.sequ:
                    return "decimal";
                case LexicCategories.splint:
                    return "string";
                case LexicCategories.bin:
                    return "binary";
                default:
                    return null;
            }
        }
    }
}
