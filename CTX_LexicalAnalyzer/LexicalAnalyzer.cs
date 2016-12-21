using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CTX_LexicalAnalyzer
{
    public class LexicalAnalyzer
    {
        private string[] _lineString;
        private readonly List<Token> _tokenList = new List<Token>();
        private readonly TokenExtractor _tokenExtractor = new TokenExtractor();

        public List<Token> LexicalAnalysis(string content)
        {
            var count = 8;
            var actualLine = 0;
            _lineString = Regex.Split(content, @"\r\n");
            foreach (var s in _lineString)
            {
                var tokens = _tokenExtractor.ReadLine(s, ++actualLine);
                foreach (var token in tokens)
                {
                    if (token.Name == string.Empty) continue;

                    foreach (var tok in _tokenList)
                    {
                        if (tok.Name != token.Name && (tok.Category != token.Category || tok.Category == "Identifier"))
                            continue;
                        token.Key = tok.Key;
                        break;
                    }

                    if (token.Key == -1)
                        token.Key = count++;

                    _tokenList.Add(token);
                }
            }

            return _tokenList;
        }
    }
}
