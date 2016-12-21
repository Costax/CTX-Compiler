using System.ComponentModel;

namespace CTX_LexicalAnalyzer
{
    public enum Rules
    {
        //id = id;
        [Description("9 1 9 3 ")]
        Rule1,
        //id = number;
        [Description("9 1 8 3 ")]
        Rule2,
        //id = ( );
        [Description("9 1 7 7 3 ")]
        Rule3,
        //number oper number
        [Description("8 2 8 ")]
        Rule4,
        //number oper number
        [Description("9 2 9 ")]
        Rule5,
        //else
        [Description("4 ")]
        Rule6,
        //id comp number
        [Description("9 5 8 ")]
        Rule7,
        //id comp id
        [Description("9 5 9 ")]
        Rule8,
        //number comp number
        [Description("8 5 8 ")]
        Rule9,
        //if ( ) || for ( )
        [Description("4 7 7 ")]
        Rule10,
        //reservedWord id = number
        [Description("4 9 1 8 ")]
        Rule11,
        //id oper
        [Description("9 2 ")]
        Rule12,
        //reservedWord id;
        [Description("4 9 3 ")]
        Rule13,
        //reservedWord id = number;
        [Description("4 9 1 8 3 ")]
        Rule14,
        //id oper id : number;
        [Description("9 2 9 5 8 ")]
        Rule15,
        //id = number
        [Description("9 1 8 ")]
        Rule16,
        //id oper id comp number
        [Description("9 5 9 2 8 ")]
        Rule17
    }

    public static class RulesExtension
    {
        public static string Rule(this Rules self)
        {
            switch (self)
            {
                case Rules.Rule1:
                    return "Rule 1";
                case Rules.Rule2:
                    return "Rule 2";
                case Rules.Rule3:
                    return "Rule 3";
                case Rules.Rule4:
                    return "Rule 4";
                case Rules.Rule5:
                    return "Rule 5";
                case Rules.Rule6:
                    return "Rule 6";
                case Rules.Rule7:
                    return "Rule 7";
                case Rules.Rule8:
                    return "Rule 8";
                case Rules.Rule9:
                    return "Rule 9";
                case Rules.Rule10:
                    return "Rule 10";
                case Rules.Rule11:
                    return "Rule 11";
                case Rules.Rule12:
                    return "Rule 12";
                case Rules.Rule13:
                    return "Rule 13";
                case Rules.Rule14:
                    return "Rule 14";
                case Rules.Rule15:
                    return "Rule 15";
                case Rules.Rule16:
                    return "Rule 16";
                case Rules.Rule17:
                    return "Rule 17";
                default:
                    return "None";
            }
        }
    }
}
