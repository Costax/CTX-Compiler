namespace CTX_LexicalAnalyzer
{
    public class Token
    {
        public string Category { get; set; }
        public string Name { get; set; }
        public int Key { get; set; }
        public int Line { get; set; }
        public string ValidType { get; set; }
        public string SetRule { get; set; }
    }
}
