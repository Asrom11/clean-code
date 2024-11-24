namespace Markdown.Parser.Interface;

public interface IMarkdownParser
{
    IEnumerable<Token> Parse(string markdownText);
}
