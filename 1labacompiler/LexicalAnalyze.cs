using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1labacompiler
{
    class LexicalAnalyzer
    {
        class Token
        {
            public string Type { get; }
            public string Value { get; }
            public int PositionStart { get; }
            public int PositionEnd { get; }

            public Token(string type, string value, int positionStart, int positionEnd)
            {
                Type = type;
                Value = value;
                PositionStart = positionStart;
                PositionEnd = positionEnd;
            }

            public override string ToString()
            {
                return $"{Type} - {Value} - с {PositionStart} по {PositionEnd} символ";
            }
        }

        class Error
        {
            public string Type { get; }
            public string Value { get; }
            public int PositionStart { get; }
            public int PositionEnd { get; }

            public Error(string type, string value, int positionStart, int positionEnd)
            {
                Type = type;
                Value = value;
                PositionStart = positionStart;
                PositionEnd = positionEnd;
            }

            public override string ToString()
            {
                return $"{Type} - {Value} - с {PositionStart} по {PositionEnd} символ";
            }
        }

        private List<Token> tokens = new List<Token>();
        private List<Error> errors = new List<Error>();

        public void Analyze(string code)
        {
            tokens.Clear();
            errors.Clear();

            string currentToken = "";
            string currentType = null;
            int positionStart = 0;
            bool insideQuotes = false;

            for (int i = 0; i < code.Length; i++)
            {
                char currentChar = code[i];

                if (char.IsLetterOrDigit(currentChar) || currentChar == '_' ||
                    (char.IsDigit(currentChar) && (currentType == null || currentType == "идентификатор" || currentType == "число")) ||
                    (currentChar == '.' && char.IsDigit(code[i + 1]) && (currentType == null || currentType == "число")) ||
                    (insideQuotes && currentChar != '\"'))
                {
                    if (currentType == null)
                    {
                        currentType = char.IsDigit(currentChar) ? "Code 8 число" : "Code 9 идентификатор";
                        positionStart = i + 1;
                    }
                    currentToken += currentChar;
                }
                else if (char.IsWhiteSpace(currentChar) || currentChar == '(' || currentChar == ')' || currentChar == '\"')
                {
                    if (currentType != null)
                    {
                        tokens.Add(new Token(insideQuotes ? "Code 7 строка" : currentType, currentToken, positionStart, i));
                        currentToken = "";
                        currentType = null;
                    }

                    if (currentChar == '\"')
                        insideQuotes = !insideQuotes;
                }
                else if (currentChar == '.')
                {
                    if (currentType != null)
                    {
                        tokens.Add(new Token(insideQuotes ? "строка" : currentType, currentToken, positionStart, i));
                        currentToken = "";
                        currentType = null;
                    }
                    tokens.Add(new Token("Code 6 Оператор точка", currentChar.ToString(), i + 1, i + 1));
                }
                else if (currentChar == '=')
                {
                    if (currentType != null)
                    {
                        tokens.Add(new Token(insideQuotes ? "строка" : currentType, currentToken, positionStart, i));
                        currentToken = "";
                        currentType = null;
                    }
                    tokens.Add(new Token("Code 1 Оператор присвоение", currentChar.ToString(), i + 1, i + 1));
                }
                else if (currentChar == '+' || currentChar == '-' ||
                         currentChar == '*' || currentChar == '/')
                {
                    if (currentType != null)
                    {
                        tokens.Add(new Token(insideQuotes ? "строка" : currentType, currentToken, positionStart, i));
                        currentToken = "";
                        currentType = null;
                    }
                    if (currentChar == '+')
                    {
                        tokens.Add(new Token("Code 2 Оператор сложения", currentChar.ToString(), i + 1, i + 1));
                    }
                    else if (currentChar == '-')
                    {
                        tokens.Add(new Token("Code 3 Оператор вычитания", currentChar.ToString(), i + 1, i + 1));
                    }
                    else if (currentChar == '*')
                    {
                        tokens.Add(new Token("Code 4 Оператор умножения", currentChar.ToString(), i + 1, i + 1));
                    }
                    else
                    {
                        tokens.Add(new Token(" Code 5 Оператор деления", currentChar.ToString(), i + 1, i + 1));
                    }
                }
                else
                {
                    errors.Add(new Error("недопустимый символ", currentChar.ToString(), i + 1, i + 1));
                }
            }

            if (currentType != null)
            {
                tokens.Add(new Token(insideQuotes ? "строка" : currentType, currentToken, positionStart, code.Length));
            }
        }

        public void DisplayResults(RichTextBox box)
        {
            foreach (Token token in tokens)
            {

                box.AppendText($"{token.Type} - {token.Value} - с {token.PositionStart} по {token.PositionEnd} символ");
                box.AppendText("\n");
            }

            foreach (Error error in errors)
            {
                 box.AppendText($"{error.Type} - {error.Value} - с {error.PositionStart} по {error.PositionEnd} символ");
            }
        }
    }

    class Work
    {
        /*static void Main()
        {
            string code = "float_format = \"{0:f}\".format(3.234e+4)";
            LexicalAnalyzer analyzer = new LexicalAnalyzer();
            analyzer.Analyze(code);
            analyzer.DisplayResults();
        }*/
    }
}
