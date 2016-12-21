using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace CTX_LexicalAnalyzer
{
    public class SemanticAnalyzer
    {
        private List<Token> _statement;
        public List<string> SemanticStatement;
        private int _totalStatements = 1, _labelNo = 1, _forProc = 0;
        private readonly Regex _regexInt = new Regex("^[0-9]*$");
        public List<string> CheckSemantic(List<IList<Token>> statementList)
        {
            SemanticStatement = new List<string>();
            statementList.Reverse();
            foreach (var line in statementList)
            {
                _statement = (List<Token>) line;
                Analyze(_statement);
            }

            return SemanticStatement;
        }

        private void Analyze(List<Token> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].Category == "Reserved word")
                {
                    if (list[list.Count - 1].Category == "Terminator")
                    {
                        if (list.Count != 3)
                        {
                            var type = list[i].ValidType;
                            if (type == "int")
                            {
                                if (_regexInt.IsMatch(list[list.Count - 2].Name))
                                {
                                    SemanticStatement.Add(
                                        $"[assign, {list[i + 1].Name}, {list[list.Count - 2].Name}, t{_totalStatements++}]");
                                    break;
                                }
                            }
                        }
                        else
                        {
                            SemanticStatement.Add($"[declare, {list[list.Count - 2].Name}, -, t{_totalStatements++}]");
                            break;
                        }
                    }
                    else if (list[list.Count - 1].Category == "Container")
                    {
                        if (list[i].Name == "sprint")
                        {
                            SemanticStatement.Add($"[label, L{_labelNo++}, -, -]");
                            list.RemoveAt(0);
                            list.RemoveAt(0);
                            list.RemoveAt(list.Count - 1);
                            ProcessToken(list);
                            break;
                        }
                        if (list[i].Name == "inc")
                        {
                            if (list[3].Name == ">")
                            {
                                SemanticStatement.Add($"[less_than, {list[4].Name}, {list[2].Name}, t{_totalStatements}]");
                            }
                            else if (list[3].Name == "<")
                            {
                                SemanticStatement.Add($"[less_than, {list[2].Name}, {list[4].Name}, t{_totalStatements}]");
                            }
                            else if (list[3].Name == ":")
                            {
                                SemanticStatement.Add($"[equals, {list[2].Name}, {list[4].Name}, t{_totalStatements}]");
                            }
                            else if (list[3].Name == ">:")
                            {
                                SemanticStatement.Add($"[less_equal, {list[4].Name}, {list[2].Name}, t{_totalStatements}]");
                            }
                            else if (list[3].Name == "<:")
                            {
                                SemanticStatement.Add($"[less_equal, {list[2].Name}, {list[4].Name}, t{_totalStatements}]");
                            }
                            else if (list[3].Name == "%")
                            {
                                if (list.Count == 8)
                                {
                                    if (list[5].Name == ":")
                                    {
                                        SemanticStatement.Add($"[module, {list[2].Name}, {list[4].Name}, t{_totalStatements}]");
                                        SemanticStatement.Add($"[equals, t{_totalStatements++}, {list[6].Name}, t{_totalStatements}]");
                                    }
                                   
                                }
                            }
                            SemanticStatement.Add($"[if_false, t{_totalStatements++}, L{_labelNo++},-]");
                            break;
                        }
                    }
                    else if (list[i].Name == "but")
                    {
                        break;
                    }
                }
                else if (list[0].Category == "Identifier")
                {
                    if (list.Count == 4)
                    {
                        SemanticStatement.Add($"[assign, {list[0].Name}, {list[2].Name}, t{_totalStatements++}]");
                        break;
                    }
                    if (list.Count == 8)
                    {
                        switch (list[4].Name)
                        {
                            case "+":
                                SemanticStatement.Add($"[sum, {list[3].Name}, {list[5].Name}, t{_totalStatements}]");
                                break;
                            case "-":
                                SemanticStatement.Add($"[sub, {list[3].Name}, {list[5].Name}, t{_totalStatements}]");
                                break;
                            case "/":
                                SemanticStatement.Add($"[divide, {list[3].Name}, {list[5].Name}, t{_totalStatements}]");
                                break;
                            case "*":
                                SemanticStatement.Add($"[times, {list[3].Name}, {list[5].Name}, t{_totalStatements}]");
                                break;
                        }

                        SemanticStatement.Add($"[assign, {list[0].Name}, t{_totalStatements++}, t{_totalStatements++}]");
                        return;
                    }
                }
            }
        }

        private void ProcessToken(List<Token> list)
        {
            var procList = list;
            var tempList = new List<Token>();
            foreach (var token in procList)
            {
                if (token.Category != "Separator")
                    tempList.Add(token);
                else
                    break;
            }

            if (_forProc < 2)
            {
                for (var i = 0; i < tempList.Count; i++)
                {
                    list.RemoveAt(0);
                }

                if (list[0].Category == "Separator")
                    list.RemoveAt(0);

                GetInformation(tempList);
                _forProc++;
                ProcessToken(procList);
                return;
            }

            GetInformation(tempList);
        }

        private void GetInformation(List<Token> tempList)
        {
            if(tempList[1].Category == "Declaratory")
            { 
                if(tempList.Count == 3)
                    SemanticStatement.Add($"[assign, {tempList[0].Name}, {tempList[2].Name}, t{_totalStatements++}]");
            }
            else if (tempList.Count > 2 && tempList[2].Category == "Declaratory")
            {
                if (tempList.Count == 4)
                    SemanticStatement.Add($"[assign, {tempList[1].Name}, {tempList[3].Name}, t{_totalStatements++}]");
            }
            else if (tempList[1].Category == "Comparator")
            {
                if (tempList.Count == 3)
                {
                    if (tempList[1].Name == "<")
                    {
                        SemanticStatement.Add($"[less_than, {tempList[0].Name}, {tempList[2].Name}, t{_totalStatements}]");
                        SemanticStatement.Add($"[if_false, t{_totalStatements++}, L{_labelNo++}, -]");
                    }
                    else if (tempList[1].Name == ">")
                    {
                        SemanticStatement.Add($"[less_than, {tempList[2].Name}, {tempList[0].Name}, t{_totalStatements}]");
                        SemanticStatement.Add($"[if_false, t{_totalStatements++}, L{_labelNo++}, -]");
                    }
                }
                else if (tempList.Count == 5)
                {
                    if (tempList[1].Name == "<:")
                    {
                        if (tempList[3].Name == "/")
                        {
                            SemanticStatement.Add($"[divide, {tempList[2].Name}, {tempList[4].Name}, t{_totalStatements}]");
                        }
                        SemanticStatement.Add($"[less_equal, {tempList[0].Name}, t{_totalStatements++}, t{_totalStatements}]");
                        SemanticStatement.Add($"[if_false, t{_totalStatements++}, L{_labelNo++}, -]");
                    }
                    
                }
            }
            else if (tempList[1].Category == "Arithmetic Op")
            {
                if (tempList.Count == 2)
                {
                    if (tempList[1].Name == "++")
                    {
                        SemanticStatement.Add($"[sum, {tempList[0].Name}, 1, t{_totalStatements++}]");
                    }
                    else if (tempList[1].Name == "--")
                    {
                        SemanticStatement.Add($"[sub, {tempList[0].Name}, 1, t{_totalStatements++}]");
                    }
                }
            }
        }
    }
}
