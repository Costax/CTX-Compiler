using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;

namespace CTX_LexicalAnalyzer
{
    public partial class MainWindow
    {
        public string FileName;
        public string FilePath;
        public List<Token> TokenList;
        public List<string> StatementList, SemanticList;
        public List<IList<Token>> StatementTokenList;
        public Token ErrorToken;
        private LexicalAnalyzer _lexicalAnalyzer;
        private SyntaxAnalyzer _syntaxAnalyzer;
        private SemanticAnalyzer _semanticAnalyzer;
        private string _content;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            TokenList = new List<Token>();
            StatementList = new List<string>();
            StatementTokenList = new List<IList<Token>>();
            _lexicalAnalyzer = new LexicalAnalyzer();
            _syntaxAnalyzer = new SyntaxAnalyzer();
            _semanticAnalyzer = new SemanticAnalyzer();

            tbOutput.Clear();
            tbSyntax.Clear();
            tbSemantic.Clear();
            ErrorToken = new Token();
            
            tbInput.CaretOffset = 0;
            tbInput.Options.HighlightCurrentLine = false;

            var ofd = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text File (*.txt)|*.txt"
            };
            var result = ofd.ShowDialog();

            if (result != true) return;

            tbOutput.Clear();

            FilePath = ofd.FileName;
            FileName = FilePath.Substring(FilePath.LastIndexOf('\\') + 1);
            lblFileName.Content = FileName;

            ReadFile(FilePath);
            TokenList = _lexicalAnalyzer.LexicalAnalysis(_content);
            _syntaxAnalyzer.CheckSyntax(TokenList);
            SemanticList = _semanticAnalyzer.CheckSemantic(StatementTokenList);

            PrintOutput();
        }

        private void ReadFile(string path)
        {
            try
            {
                using (var sr = new StreamReader(path))
                {
                    _content = sr.ReadToEnd();
                    tbInput.Text = _content;
                }
            }
            catch (Exception e) { tbOutput.Text = "The file could not be read. " + e.Message; }
        }

        private void PrintOutput()
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.01) };
            var i = 0;
            var x = 0;
            StatementList.Reverse();
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                tbOutput.Text += $" [ {TokenList[i].Category} | {TokenList[i].Name} ] \r\n";
                tbOutput.ScrollToEnd();

                if (x < StatementList.Count)
                {
                    tbSyntax.Text += $" {StatementList[x]} \r\n";
                    tbSyntax.ScrollToEnd();
                    x++;
                }

                i++;

                if (i != TokenList.Count) return;

                timer.Stop();

                while (x < StatementList.Count - 1)
                {
                    tbSyntax.Text += $" {StatementList[x]} \r\n";
                    tbSyntax.ScrollToEnd();
                    x++;
                }

                foreach (var s in SemanticList)
                {
                    tbSemantic.Text += $" {s} \r\n";
                    tbSemantic.ScrollToEnd();
                }

                if (ErrorToken != null && ErrorToken.Line > 0)
                {
                    tbConsoleOutput.Text = "One or many syntax error's have been found. Please review your code. \r\n" +
                                           $"Error in line { ErrorToken.Line} [Token ' {ErrorToken.Name} ']";
                    tbInput.CaretOffset = tbInput.Document.GetLineByNumber(ErrorToken.Line).Offset;
                    tbInput.Options.HighlightCurrentLine = true;
                }
                else
                    tbConsoleOutput.Text = "Lexical and Syntax analysis completed. (100%)";
            };
        }
    }
}
