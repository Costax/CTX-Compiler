using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CTX_LexicalAnalyzer
{
    public class SyntaxAnalyzer
    {
        private List<Rules> _rules = new List<Rules>();
        private string _statement, setRule, ant;
        private readonly MainWindow _mainWindow = (MainWindow) Application.Current.MainWindow;

        public void CheckSyntax(List<Token> tokens)
        {
            var tokProcessStack = new Stack<Token>();
            var tokArithStack = new Stack<Token>();
            var tokParamStack = new Stack<Token>();
            var lineStack = new Stack<Token>();
            var currentLine = 0;

            tokens.Reverse();

            foreach (var token in tokens)
            {
                try
                {
                    if (token.Name.Equals(LexicCategories.EndProcess.ToDescriptionString()))
                        tokProcessStack.Push(token);

                    if (token.Name.Equals(LexicCategories.StartProcess.ToDescriptionString()))
                        tokProcessStack.Pop();

                    if (token.Name.Equals(LexicCategories.EndArithProcess.ToDescriptionString()))
                        tokArithStack.Push(token);

                    if (token.Name.Equals(LexicCategories.StartArithProcess.ToDescriptionString()))
                        tokArithStack.Pop();

                    if (token.Name.Equals(LexicCategories.EndParameter.ToDescriptionString()))
                        tokParamStack.Push(token);

                    if (token.Name.Equals(LexicCategories.StartParameter.ToDescriptionString()))
                        tokParamStack.Pop();

                    if (currentLine == 0 | token.Line == currentLine)
                    {
                        currentLine = token.Line;
                        lineStack.Push(token);
                    }
                    else
                    {
                        if (EvaluateLine(lineStack))
                        {
                            lineStack.Clear();
                            lineStack.Push(token);
                            currentLine = token.Line;
                        }
                        else
                            throw new Exception();
                    }
                }
                catch (Exception e)
                {
                    _mainWindow.ErrorToken = token;
                    return;
                }
            }

            if (tokParamStack.Count > 0)
                _mainWindow.ErrorToken = tokParamStack.Pop();

            if (tokArithStack.Count > 0)
                _mainWindow.ErrorToken = tokArithStack.Pop();

            if (tokProcessStack.Count > 0)
                _mainWindow.ErrorToken = tokProcessStack.Pop();

            tokens.Reverse();
        }

        private bool EvaluateLine(IEnumerable<Token> lineStack)
        {
            var stack = lineStack as IList<Token> ?? lineStack.ToList();

            if (stack.Count == 1 && stack.First().Key == 7)
                return true;

            if (stack[0].Name != "sprint" && stack[0].Name != "inc")
            {
                if (ant != "inc" && (stack.Count != 3 || stack[stack.Count - 1].Category == "Terminator"))
                {
                    _mainWindow.StatementTokenList.Add(stack);
                }
            }
            else if (stack[0].Name == "sprint" && ant != "sprint")
                _mainWindow.StatementTokenList.Add(stack);
            else if(stack[0].Name == "inc" && ant != "inc")
                _mainWindow.StatementTokenList.Add(stack);

            ant = stack[0].Name;

            _rules = Enum.GetValues(typeof(Rules)).Cast<Rules>().ToList();
            _statement = stack.Select(token => token.Key >= 9 ? 9 : token.Key).Aggregate(string.Empty, (current, key) => current + $"{key} ");
            string content = null;
            string updated = null;

            if (_statement.Contains("7"))
            {
                if (_statement.Where((t, i) => _statement.Substring(i, 1) == "7").Count()%2 == 0)
                {
                    var prev = _statement.Substring(0, _statement.IndexOf("7", StringComparison.Ordinal) + 1);
                    content = _statement.Substring(_statement.IndexOf("7", StringComparison.Ordinal) + 2,
                        _statement.LastIndexOf("7", StringComparison.Ordinal) -
                        _statement.IndexOf("7", StringComparison.Ordinal) - 2);
                    var post = _statement.Substring(_statement.LastIndexOf("7", StringComparison.Ordinal));
                    updated = prev + " " + post;
                }

                if (!ValidateRule(updated)) return false;

                var contentTokens = new List<Token>();

                if (content == null) return false;

                var contentStrings = content.Split(' ');

                foreach (var contentString in contentStrings)
                {
                    foreach (var token in stack)
                    {
                        var temp = token.Key >= 10 ? 9 : token.Key;

                        if (temp.ToString() != contentString) continue;

                        contentTokens.Add(token);
                        break;
                    }
                }
                return EvaluateLine(contentTokens);
            }

            if (!_statement.Contains("6")) return ValidateRule(_statement);

            if (_statement.Where((t, i) => _statement.Substring(i, 1) == "6").Count() != 2) return false;

            var pre = _statement.Substring(0, _statement.IndexOf("6", StringComparison.Ordinal));
            var middle = _statement.Substring(_statement.IndexOf("6", StringComparison.Ordinal) + 2,
                _statement.LastIndexOf("6", StringComparison.Ordinal) -
                _statement.IndexOf("6", StringComparison.Ordinal) - 2);
            var postSix = _statement.Substring(_statement.LastIndexOf("6", StringComparison.Ordinal) + 2);

            if (!ValidateRule(pre)) return false;
            return ValidateRule(middle) && ValidateRule(postSix);
            
        }

        private bool ValidateRule(string statement)
        {
            foreach (var rule in _rules)
            {
                if (rule.ToDescriptionString() != statement) continue;
                _mainWindow.StatementList.Add(rule.Rule());
                _mainWindow.StatementList.Add(statement);
                setRule = rule.ToDescriptionString();
                return true;
            }
            return false;
        }
    }
}