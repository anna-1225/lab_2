using System;
using System.Collections.Generic;
using System.Text;

namespace new2026
{
    public class Token
    {
        public int Code { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
        public int Position { get; set; }
        public bool IsError { get; set; }
    }

    public class Scanner
    {
        private readonly HashSet<char> _letters = new HashSet<char>("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_");
        private readonly HashSet<char> _digits = new HashSet<char>("0123456789");
        private readonly HashSet<char> _operators = new HashSet<char>("=<>!+-*/");
        private readonly HashSet<char> _delimiters = new HashSet<char>(";,.(){}[]");

        private readonly HashSet<string> _keywords = new HashSet<string> { "if", "else" };

        public List<Token> Analyze(string text)
        {
            var tokens = new List<Token>();
            int line = 1;
            int pos = 0;

            while (pos < text.Length)
            {
                char c = text[pos];

                if (c == ' ' || c == '\t')
                {
                    pos++;
                    continue;
                }

                if (c == '\n')
                {
                    line++;
                    pos++;
                    continue;
                }

                int startPos = pos;

                if (_digits.Contains(c))
                {
                    string num = "";
                    while (pos < text.Length && (_digits.Contains(text[pos]) || text[pos] == '.'))
                        num += text[pos++];

                    tokens.Add(new Token
                    {
                        Code = 3,
                        Type = "Число",
                        Value = num,
                        Line = line,
                        Position = startPos
                    });
                }

                else if (_letters.Contains(c))
                {
                    string word = "";
                    while (pos < text.Length && (_letters.Contains(text[pos]) || _digits.Contains(text[pos])))
                        word += text[pos++];

                    if (_keywords.Contains(word))
                        tokens.Add(new Token { Code = 1, Type = "Ключевое слово", Value = word, Line = line, Position = startPos });
                    else
                        tokens.Add(new Token { Code = 2, Type = "Идентификатор", Value = word, Line = line, Position = startPos });
                }

                else if (_operators.Contains(c))
                {
                    if (pos + 1 < text.Length && _operators.Contains(text[pos + 1]) &&
                        (c == '=' || c == '>' || c == '<' || c == '!'))
                    {
                        string op = c.ToString() + text[pos + 1];
                        tokens.Add(new Token { Code = 4, Type = "Оператор", Value = op, Line = line, Position = startPos });
                        pos += 2;
                    }
                    else
                    {
                        tokens.Add(new Token { Code = 4, Type = "Оператор", Value = c.ToString(), Line = line, Position = startPos });
                        pos++;
                    }
                }

                else if (_delimiters.Contains(c))
                {
                    tokens.Add(new Token { Code = 5, Type = "Разделитель", Value = c.ToString(), Line = line, Position = startPos });
                    pos++;
                }

                else
                {
                    tokens.Add(new Token
                    {
                        Code = 99,
                        Type = "ОШИБКА",
                        Value = c.ToString(),
                        Line = line,
                        Position = startPos,
                        IsError = true
                    });
                    pos++;
                }
            }

            return tokens;
        }
    }
}