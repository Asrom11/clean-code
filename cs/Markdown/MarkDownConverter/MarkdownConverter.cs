using System.Text;
using Markdown.interfaces;

namespace Markdown;

public class MarkdownConverter : IMarkdownConverter
{
    private readonly List<IHtmlTagConverter> tagConverters = CreateTagConverters();

    private static List<IHtmlTagConverter> CreateTagConverters()
    {
        return
        [
            new HeaderTagConverter(),
            new ItalicTagConverter(),
            new StrongTagConverter()
        ];
    }

    public string Convert(IEnumerable<Token> tokens)
    {
        IList<Token> convertedTokens = tokens.ToArray();
        var result = new StringBuilder();

        foreach (var tagConverter in tagConverters)
        {
            convertedTokens = tagConverter.ConvertToHtml(convertedTokens);
        }

        foreach (var text in convertedTokens.Select(token => token.Text))
        {
            result.Append(text);
        }

        return result.ToString();
    }
}