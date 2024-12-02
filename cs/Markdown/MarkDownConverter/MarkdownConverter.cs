using System.Text;
using Markdown.interfaces;

namespace Markdown
{
    public class MarkdownConverter : IMarkdownConverter
    {
        public string Convert(IEnumerable<Token> tokens)
        {
            var result = new StringBuilder();
            var tagStack = new Stack<TokenType>();

            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Text:
                        result.Append(System.Net.WebUtility.HtmlEncode(token.Text));
                        break;

                    case TokenType.Strong:
                    case TokenType.Italic:
                        HandleFormattingTag(token, tagStack, result);
                        break;

                    case TokenType.Header:
                        HandleHeaderTag(token, tagStack, result);
                        break;
                    case TokenType.Link:
                        HandleLinkToken(token, result);
                        break;
                }
            }
            
            while (tagStack.Count > 0)
            {
                var openTag = tagStack.Pop();
                result.Append(GetClosingTag(openTag));
            }

            return result.ToString();
        }
        
        private void HandleLinkToken(Token token, StringBuilder result)
        {
            result.Append($"<a href=\"{token.Url}\">{token.Text}</a>");
        }
        
        private void HandleFormattingTag(Token token, Stack<TokenType> tagStack, StringBuilder result)
        {
            if (token.State == TagState.Open)
            {
                result.Append(GetOpeningTag(token.Type));
                tagStack.Push(token.Type);
            }
            else 
            {
                if (tagStack.Count > 0 && tagStack.Peek() == token.Type)
                {
                    result.Append(GetClosingTag(token.Type));
                    tagStack.Pop();
                }
                else
                {
                    result.Append(System.Net.WebUtility.HtmlEncode(token.Text));
                }
            }
        }

        private void HandleHeaderTag(Token token, Stack<TokenType> tagStack, StringBuilder result)
        {
            if (token.State == TagState.Open)
            {
                result.Append($"<h{token.Level}>");
                tagStack.Push(TokenType.Header);
                return;
            }

            if (tagStack.Count > 0 && tagStack.Peek() == TokenType.Header)
            {
                result.Append($"</h{token.Level}>");
                tagStack.Pop();
                return;
            }

            result.Append(System.Net.WebUtility.HtmlEncode(token.Text));
        }

        private string GetOpeningTag(TokenType type)
        {
            return type switch
            {
                TokenType.Strong => "<strong>",
                TokenType.Italic => "<em>",
                _ => string.Empty
            };
        }

        private string GetClosingTag(TokenType type)
        {
            return type switch
            {
                TokenType.Strong => "</strong>",
                TokenType.Italic => "</em>",
                _ => string.Empty
            };
        }
    }
}