using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CTX_LexicalAnalyzer
{
    public class TokenExtractor
    {
        private string _builtToken = string.Empty;
        private string [] _codeLine;
        private List<LexicCategories> _lexems;
        private List<Token> _tokens;
        private readonly Regex _regexLetter = new Regex("^[a-zA-Z]*$");
        private readonly Regex _regexInt = new Regex("^[0-9]*$");
        private int _actualLine = 0;

        public List<Token> ReadLine(string codeLine, int actualLine)
        {
            _actualLine = actualLine;
            _tokens = new List<Token>();
            _codeLine = Regex.Split(codeLine, @"\s+");
            _lexems = Enum.GetValues(typeof(LexicCategories)).Cast<LexicCategories>().ToList();
            GetToken();
            return _tokens;
        }

        private void GetToken()
        {
            foreach (var word in _codeLine)
            {
                var added = false;
                _builtToken = string.Empty;

                if(word == "")
                    continue;

                foreach (var lexem in _lexems)
                {
                    if (_builtToken == lexem.ToDescriptionString())
                    {
                        AddToken(_builtToken);
                        break;
                    }

                    if (_regexInt.IsMatch(word))
                    {
                        AddToken(word);
                        break;
                    }

                    if (!word.Contains(lexem.ToDescriptionString())) continue;

                    if (word == lexem.ToDescriptionString())
                    {
                        AddToken(word);
                        added = true;
                        break;
                    }

                    if (_lexems.Any(lex => word == lex.ToDescriptionString()))
                    {
                        AddToken(word);
                        added = true;
                    }

                    if (added)
                        break;

                    _builtToken = string.Empty;

                    foreach (var c in word)
                    {
                        if (_regexInt.IsMatch(c.ToString()))
                        {
                            _builtToken += c.ToString();
                            continue;
                        }

                        if (c.ToString() == lexem.ToDescriptionString())
                        {
                            AddToken(_builtToken);
                            AddToken(c.ToString());
                            if (word.Substring(word.IndexOf(c.ToString(), StringComparison.Ordinal)).Length == 1)
                                break;
                            continue;
                        }

                        if (_builtToken == lexem.ToDescriptionString())
                        {
                            AddToken(_builtToken);
                            break;
                        }

                        if (_regexLetter.IsMatch(c.ToString()))
                            _builtToken += c.ToString();

                        if (_builtToken == lexem.ToDescriptionString())
                        {
                            AddToken(_builtToken);
                            continue;
                        }

                        if (c.ToString() != LexicCategories.Plus.ToDescriptionString() && c.ToString() != LexicCategories.Minus.ToDescriptionString())
                            continue;

                        if (_builtToken != c.ToString())
                        {
                            AddToken(_builtToken);
                            _builtToken = string.Empty;
                        }

                        _builtToken += c.ToString();

                        if (_builtToken == LexicCategories.Incremental.ToDescriptionString())
                        {
                            AddToken(_builtToken);
                            if (word.Substring(word.IndexOf(_builtToken, StringComparison.Ordinal) + 1).Length == 1)
                                goto StepOver;
                        }
                        else if(_builtToken == LexicCategories.Decremental.ToDescriptionString())
                        {
                            AddToken(_builtToken);
                            if (word.Substring(word.IndexOf(_builtToken, StringComparison.Ordinal) + 1).Length == 1)
                                goto StepOver;
                        }
                    }
                }

                StepOver:

                if (added == false && _regexLetter.IsMatch(word))
                    AddToken(word);
            }
        }

        private void AddToken(string word)
        {
            var newToken = new Token
            {
                Name = word,
                Key = -1,
                Line = _actualLine
            };
            foreach (var lexem in _lexems)
            {
                if (word != lexem.ToDescriptionString())
                    continue;
                newToken.Category = lexem.Category();
                newToken.Key = lexem.Key();
                if(newToken.Key == 4)
                    newToken.ValidType = lexem.GetValidType();
                break;
            }

            if (newToken.Category == null)
            {
                if (_regexInt.IsMatch(word))
                {
                    newToken.Category = "Number";
                    newToken.Key = 8;
                }
                else
                {
                    newToken.Category = "Identifier";
                    newToken.Key = 9;
                }
            }
            _tokens.Add(newToken);
        }
    }
}
