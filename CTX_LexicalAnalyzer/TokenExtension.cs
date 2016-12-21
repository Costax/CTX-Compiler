using System.ComponentModel;

namespace CTX_LexicalAnalyzer
{
    public static class MyEnumExtensions
    {
        public static string ToDescriptionString(this LexicCategories val)
        {
            var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
